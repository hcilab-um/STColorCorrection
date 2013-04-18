/*
 *
 * Copyright (c) 2009, 2011, Oracle and/or its affiliates. All rights reserved.

 *
 * This file is available and licensed under the following license:
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 *  * Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 *  * Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *  * Neither the name of Oracle Corporation nor the names of its contributors
 *    may be used to endorse or promote products derived from this software
 *    without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
package example.mmademo;

import javax.microedition.lcdui.*;
import javax.microedition.media.*;
import javax.microedition.media.control.FramePositioningControl;
import javax.microedition.media.control.RateControl;
import javax.microedition.media.control.VideoControl;
import javax.microedition.media.control.VolumeControl;
import java.io.InputStream;

/**
 * Play/Capture Video in a Canvas
 */
public class VideoCanvas extends Canvas
    implements Runnable, CommandListener, PlayerListener {

    private static Player player = null;
    private static boolean isCapturePlayer;
    private int idx = 0;
    private boolean fsmode = false;
    private Display parentDisplay;
    private long duration;
    private long lastTime = -1;
    private Thread sliderThread;
    private Command backCommand = new Command ("Back", Command.BACK, 1);
    private Command playCommand = new Command ("Play", Command.ITEM, 1);
    private Command snapCommand = new Command ("Snapshot", Command.ITEM, 1);
    private Command pauseCommand = new Command ("Pause", Command.ITEM, 10);
    private VolumeControl vc;
    private RateControl rc;
    private VideoControl vidc;
    private FramePositioningControl fpc;
    private int CB_Y = 0;   // Control Bar Y location
    private static final int CB_H = 8;   // Control Bar Height
    private static final int TH_W = 6;   // Slider Thumb Width
    private static final int VB_W = 10;  // Video Border Width
    private static final int VB_H = 8;   // Video Border Height

    private boolean vis = true;

    private int canvasW;    // Canvas Width
    private int canvasH;    // Canvas Height

    private int videoW;     // Video Display Area Width
    private int videoH;     // Video Display Area Height

    // pause/resume support
    private boolean suspended = false;
    private boolean restartOnResume = false;
    private long restartMediaTime;


    public VideoCanvas (Display parentDisplay) {
        this.idx = 0;
        this.parentDisplay = parentDisplay;
        setSize (getWidth (), getHeight ());
        initialize ();
    }

    public void paint (Graphics g) {
        // Only video player uses the screen in fullscreen mode
        if (fsmode) return;

        // Draw the GUI and media time slider
        g.setColor (0x9090E0);
        g.fillRect (0, 0, canvasW, canvasH);
        g.setColor (0x202050);
        //Video Border
        g.drawLine (VB_W - 1, VB_H - 1, videoW + VB_W - 1, VB_H - 1);
        g.drawLine (VB_W - 1, VB_H - 1, VB_W - 1, videoH + VB_H - 1);
        //Control Bar
        if (sliderThread != null) {
            g.drawLine (VB_W - 1, CB_Y, videoW + VB_W - 1, CB_Y);
            g.drawLine (VB_W - 1, CB_Y, VB_W - 1, CB_Y + CB_H);
        }
        g.setColor (0xD0D0FF);
        //Video Border
        g.drawLine (videoW + VB_W, VB_H, videoW + VB_W, videoH + VB_H);
        g.drawLine (videoW + VB_W, videoH + VB_H, VB_W, videoH + VB_H);
        //Control Bar
        if (sliderThread != null) {
            g.drawLine (
                videoW + VB_W, CB_Y + 1,
                videoW + VB_W, CB_Y + CB_H + 1);
            g.drawLine (
                videoW + VB_W, CB_Y + CB_H + 1,
                VB_H, CB_Y + CB_H + 1);
        }
        if (sliderThread != null) {
            int p = time2pix (lastTime);
            g.drawLine (VB_W + p, CB_Y + 1, VB_W + p, CB_Y + CB_H);
            g.drawLine (VB_W + p, CB_Y + 1, VB_W + p + TH_W - 1, CB_Y + 1);
            g.setColor (0x202050);
            g.drawLine (VB_W + p + 1, CB_Y + CB_H, VB_W + p + TH_W, CB_Y + CB_H);
            g.drawLine (
                VB_W + p + TH_W, CB_Y + CB_H,
                VB_W + p + TH_W, CB_Y + 1);
            g.setColor (0xA0A0FF);
            g.fillRect (VB_W + p + 1, CB_Y + 2, TH_W - 2, CB_H - 2);
            g.setColor (0);
            g.drawString (
                "00:00", VB_W, CB_Y + CB_H + 4,
                Graphics.TOP | Graphics.LEFT);
            g.drawString (
                time2String (lastTime) + "/" + time2String (duration),
                VB_W + videoW, CB_Y + CB_H + 4,
                Graphics.TOP | Graphics.RIGHT);
        }
    }

    private void setSize (int width, int height) {
        canvasW = width;
        canvasH = height;
        CB_Y = canvasH - CB_H - VB_H - 20;
        videoW = canvasW - VB_W * 2;
        videoH = CB_Y - VB_H * 2;
    }

    private void initialize () {
        addCommand (backCommand);
        addCommand (snapCommand);
        setCommandListener (this);
    }

    private int time2pix (long time) {
        int t2p = (int) ((time * (videoW - 1 - TH_W)) / duration);
        return t2p;
    }

    private String time2String (long time) {
        time = time / 1000000;
        String strTime = "" + (time % 10);
        time = time / 10;
        strTime = (time % 6) + strTime;
        time = time / 6;
        strTime = (time % 10) + ":" + strTime;
        time = time / 10;
        strTime = (time % 6) + strTime;
        time = time / 6;
        return strTime;
    }

    public void run () {
        while (player != null && player.getState () != Player.CLOSED) {
            try {
                long time = player.getMediaTime ();
                if (time2pix (time) != time2pix (lastTime)) {
                    lastTime = time;
                    repaint (
                        VB_W - 2, CB_Y,
                        videoW + 2, CB_H + 2 + 20);
                }
                // sleep for 250 millis
                // if suspended, sleep until the MIDlet is restarted
                do {
                    Thread.sleep (250);
                }
                while (player != null && suspended);
            }
            catch (Exception e) {
                System.err.println ("In run(): " + e);
                break;
            }
        }
    }

    /*
     * Respond to commands, including back
     */
    public void commandAction (Command c, Displayable s) {
        if (s == this) {
            if (c == backCommand) {
                close ();
                parentDisplay.setCurrent (VideoTest.getList ());
            }
            else if (vidc != null && c == snapCommand) {
                doSnapshot ();
            }
            else if (vidc == null && c == pauseCommand) {
                removeCommand (pauseCommand);
                addCommand (playCommand);
                pause ();
            }
            else if (vidc == null && c == playCommand) {
                start ();
                removeCommand (playCommand);
                addCommand (pauseCommand);
            }
        }
    }

    /*
     * Process clicking in slider region
     */
    protected void pointerPressed (int x, int y) {
        if (duration == Player.TIME_UNKNOWN || isCapturePlayer)
            return;
        if (y >= CB_Y && y < CB_Y + CB_H) {
            if (x >= VB_W - 2 && x <= VB_W - 2 + videoW) {
                x = x - (VB_W + 1);
                if (x < 0)
                    x = 0;
                if (x > videoW - TH_W)
                    x = videoW - TH_W;
                long time = (duration * x) / (videoW - TH_W);
                try {
                    player.setMediaTime (time);
                }
                catch (MediaException me) {
                    System.out.println (me);
                }
            }
        }
    }

    protected void pointerDragged (int x, int y) {
        pointerPressed (x, y);
    }

    public void open (String url) {
        try {
            synchronized (this) {
                if (player == null) {
                    if (url.startsWith ("resource:")) {
                        InputStream ins = getClass ().getResourceAsStream (url.substring (9));
                        String ct = Utils.guessContentType (url);
                        player = Manager.createPlayer (ins, ct);
                    }
                    else {
                        player = Manager.createPlayer (url);
                    }
                    player.addPlayerListener (this);
                    isCapturePlayer = url.startsWith ("capture:");
                    if (url.endsWith ("gif")) { // do not show snapshot (Snapshots for gif are not supported).
                        removeCommand (snapCommand);
                    }
                }
            }
            player.realize ();
            if ((vidc = (VideoControl) player.getControl ("VideoControl")) != null) {
                vidc.initDisplayMode (VideoControl.USE_DIRECT_VIDEO, this);
                int frameW = vidc.getSourceWidth ();
                int frameH = vidc.getSourceHeight ();
                // Clip the video to available area
                if (frameW > videoW)
                    frameW = videoW;
                if (frameH > videoH)
                    frameH = videoH;
                int frameX = (videoW - frameW) / 2 + VB_W;
                int frameY = (videoH - frameH) / 2 + VB_H;
                vidc.setDisplayLocation (frameX, frameY);
                vidc.setDisplaySize (frameW, frameH);
                vidc.setVisible (true);
            }
            Control[] controls = player.getControls ();

            for (int i = 0; i < controls.length; i++) {
                if (controls[i] instanceof VolumeControl)
                    vc = (VolumeControl) controls[i];
                if (controls[i] instanceof RateControl)
                    rc = (RateControl) controls[i];
                if (controls[i] instanceof FramePositioningControl)
                    fpc = (FramePositioningControl) controls[i];
            }
            player.prefetch ();
            if (vidc == null)
                addCommand (pauseCommand);
        }
        catch (Exception e) {
            System.err.println (e);
            close ();
        }
    }

    public void start () {
        if (player == null)
            return;
        try {
            duration = player.getDuration ();
            player.start ();
            if (duration != Player.TIME_UNKNOWN) {
                sliderThread = new Thread (this);
                sliderThread.start ();
            }
        }
        catch (Exception e) {
            System.err.println (e);
            close ();
        }
    }

    public void close () {
        synchronized (this) {
            pause ();
            if (player != null) {
                player.close ();
                player = null;
            }
        }
        VideoTest.getInstance ().nullPlayer ();
    }

    public void pause () {
        if (player != null) {
            try {
                player.stop ();
            }
            catch (MediaException me) {
                System.err.println (me);
            }
        }
    }

    public void playerUpdate (Player plyr, String evt, Object evtData) {
        if (evt == END_OF_MEDIA) {
            try {
                player.setMediaTime (0);
                player.start ();
            }
            catch (MediaException me) {
                System.err.println (me);
            }
        }
    }

    private void doSnapshot () {
        new SnapshotThread ().start ();
    }

    class SnapshotThread extends Thread {
        final Canvas tThis = (Canvas) VideoCanvas.this;

        public void run () {
            try {
                byte[] snap = vidc.getSnapshot ("encoding=jpeg");
                if (snap != null) {
                    Image im = Image.createImage (snap, 0, snap.length);
                    Alert al = new Alert (
                        "Snapshot",
                        "Here's the snap",
                        im,
                        AlertType.INFO);
                    al.setTimeout (2000);
                    parentDisplay.setCurrent (al, tThis);
                }
            }
            catch (MediaException me) {
                System.err.println (me);
            }
        }
    }

    public synchronized void stopVideoCanvas () {
        // stop & deallocate
        player.deallocate ();
    }

    /* Handle the different keys pressed on the phone GUI emulator */
    public void keyPressed (int keyCode) {
        int cr, cv;
        switch (keyCode) {
        case Canvas.KEY_NUM4:
            cr = rc.getRate ();
            cr -= 10000;
            cr = rc.setRate (cr);
            break;
        case Canvas.KEY_NUM6:
            cr = rc.getRate ();
            cr += 10000;
            cr = rc.setRate (cr);
            break;
        case Canvas.KEY_STAR:
            if (vc != null) {
                cv = vc.getLevel ();
                cv -= 10;
                cv = vc.setLevel (cv);
            }
            break;
        case Canvas.KEY_NUM0:
            if (vc != null) {
                vc.setMute (!vc.isMuted ());
            }
            break;
        case Canvas.KEY_POUND:
            if (vc != null) {
                cv = vc.getLevel ();
                cv += 10;
                cv = vc.setLevel (cv);
            }
            break;
        case Canvas.KEY_NUM7:
            if (fpc != null) {
                fpc.skip (-1);
            }
            break;
        case Canvas.KEY_NUM5:
            try {
                player.stop ();
                if (!isCapturePlayer) {
                    player.setMediaTime (0);
                }
                player.deallocate ();
            }
            catch (MediaException me) {
                System.err.println (me);
            }
            break;
        case Canvas.KEY_NUM9:
            if (fpc != null) {
                fpc.skip (1);
            }
            break;
        case Canvas.KEY_NUM2:
            try {
                if (player.getState () == Player.STARTED)
                    player.stop ();
                else
                    player.start ();
            }
            catch (Exception e) {
                System.err.println (e);
            }
            break;
        case Canvas.KEY_NUM8:
            try {
                // Full screen
                if (vidc != null)
                    vidc.setDisplayFullScreen (fsmode = !fsmode);

                repaint ();
            }
            catch (MediaException me) {
                System.err.println (me);
            }
            break;
        case Canvas.KEY_NUM1:
        case Canvas.KEY_NUM3:
            if (!isCapturePlayer) {
                long mTime = player.getMediaTime ();
                long duration = player.getDuration ();
                if (duration == Player.TIME_UNKNOWN || mTime == Player.TIME_UNKNOWN)
                    return;
                try {
                    if (keyCode == Canvas.KEY_NUM3) {
                        // Jump forward 10%
                        mTime += duration / 10;
                        if (mTime > duration)
                            mTime = duration;
                        player.setMediaTime (mTime);
                    }
                    else if (keyCode == Canvas.KEY_NUM1) {
                        mTime -= duration / 10;
                        if (mTime < 0)
                            mTime = 0;
                        player.setMediaTime (mTime);
                    }
                    else
                        return;
                }
                catch (MediaException me) {
                    System.err.println (me);
                }
            }
            break;
            /* Code to move the video around using cursor keys */
        default:
            int game = getGameAction (keyCode);
            int x = vidc.getDisplayX ();
            int y = vidc.getDisplayY ();
            if (game == UP)
                vidc.setDisplayLocation (x, y - 10);
            else if (game == DOWN)
                vidc.setDisplayLocation (x, y + 10);
            else if (game == LEFT)
                vidc.setDisplayLocation (x - 10, y);
            else if (game == RIGHT)
                vidc.setDisplayLocation (x + 10, y);
            else if (game == FIRE)
                vidc.setVisible (vis = !vis);
            repaint ();
            break;
        }
    }


    /**
     * Deallocate the player and the display thread.
     * Some VM's may stop players and threads
     * on their own, but for consistent user
     * experience, it's a good idea to explicitly
     * stop and start resources such as player
     * and threads.
     */
    public synchronized void pauseApp () {
        suspended = true;
        if (player != null && player.getState () >= Player.STARTED) {
            // player was playing, so stop it and release resources.
            if (!isCapturePlayer) {
                restartMediaTime = player.getMediaTime ();
            }
            player.deallocate ();
            // make sure to restart upon resume
            restartOnResume = true;
        }
        else {
            restartOnResume = false;
        }
    }


    /**
     * If the player was playing when the MIDlet was paused,
     * then the player will be restarted here.
     */
    public synchronized void startApp () {
        suspended = false;
        if (player != null && restartOnResume) {
            try {
                player.prefetch ();
                if (!isCapturePlayer) {
                    try {
                        player.setMediaTime (restartMediaTime);
                    }
                    catch (MediaException me) {
                        System.err.println (me);
                    }
                }
                player.start ();
            }
            catch (MediaException me) {
                System.err.println (me);
            }
        }
        restartOnResume = false;
    }

    /**
     * React on sizeChanged event.
     */
    protected void sizeChanged (int width, int height) {
        setSize (width, height);
        if (vidc != null) {
            int frameX = (videoW - vidc.getSourceWidth ()) / 2 + VB_W;
            int frameY = (videoH - vidc.getSourceHeight ()) / 2 + VB_H;
            vidc.setDisplayLocation (frameX, frameY);
        }
    }

}
