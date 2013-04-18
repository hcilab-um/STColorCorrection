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
import javax.microedition.media.control.GUIControl;
import javax.microedition.media.control.RateControl;
import javax.microedition.media.control.VideoControl;
import javax.microedition.media.control.VolumeControl;
import java.io.InputStream;

/**
 * Play Video/Capture in a Form using MMAPI
 */
public class VideoPlayer extends Form
    implements Runnable, CommandListener, PlayerListener {

    private static final String TITLE_TEXT = "MMAPI Video Player";

    private static Player player = null;
    private static boolean isCapturePlayer;

    private static Image logo = null;
    private int idx = 0;
    private Display parentDisplay;
    private long duration;
    private final Command backCommand = new Command ("Back", Command.BACK, 1);
    private final Command playCommand = new Command ("Play", Command.ITEM, 1);
    private final Command snapCommand = new Command ("Snapshot", Command.ITEM, 1);
    private final Command pauseCommand = new Command ("Pause", Command.ITEM, 10);
    private Item videoItem;
    private StringItem status;
    private StringItem audioStatus;
    private StringItem time;
    private VolumeControl vc;
    private RateControl rc;
    private Thread th;
    private int currentVolume;
    private boolean muted;
    private int currentRate = 100000;
    private VideoControl vidc;

    // pause/resume support
    private boolean suspended = false;
    private boolean restartOnResume = false;
    private long restartMediaTime;

    public VideoPlayer (Display parentDisplay) {
        super (TITLE_TEXT);
        this.idx = 0;
        this.parentDisplay = parentDisplay;
        initialize ();
    }

    void initialize () {
        addCommand (backCommand);
        addCommand (snapCommand);
        setCommandListener (this);

        try {
            if (logo == null)
                logo = Image.createImage ("/icons/_dukeok2.png");
        }
        catch (Exception ex) {
            logo = null;
        }
        if (logo == null)
            System.out.println ("can not load _dukeok2.png");

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
            else if (videoItem != null && c == snapCommand) {
                doSnapshot ();
            }
            else if (videoItem == null && c == pauseCommand) {
                removeCommand (pauseCommand);
                addCommand (playCommand);
                pause ();
            }
            else if (videoItem == null && c == playCommand) {
                start ();
                removeCommand (playCommand);
                addCommand (pauseCommand);
            }
        }
    }

    public void run () {
        while (player != null) {

            // sleep 200 millis. If suspended,
            // sleep until MIDlet is restarted
            do {
                try {
                    Thread.sleep (200);
                }
                catch (InterruptedException ie) {
                }
            }
            while (player != null && suspended);

            synchronized (this) {
                if (player == null)
                    return;
                if (vc != null) {
                    if (vc.getLevel () != currentVolume || vc.isMuted () != muted) {
                        muted = vc.isMuted ();
                        currentVolume = vc.getLevel ();
                        audioStatus.setText (
                            "Volume: " + currentVolume + "% " +
                                (muted ? " (muted)" : ""));
                    }
                }
                if (rc != null) {
                    if (rc.getRate () != currentRate) {
                        currentRate = rc.getRate ();
                        updateStatus ();
                    }
                }
                long k = player.getMediaTime ();
                time.setText ("Pos: " + (k / 1000000) + "." + ((k / 10000) % 100));
            }
        }
    }

    public void open (String url) {
        System.out.println ("OPEN:" + url);
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
                videoItem = (Item) vidc.initDisplayMode (VideoControl.USE_GUI_PRIMITIVE, null);
            }
            else if (logo != null) {
                append (new ImageItem ("", logo, ImageItem.LAYOUT_CENTER, ""));
            }
            Control[] controls = player.getControls ();

            for (int i = 0; i < controls.length; i++) {
                if (controls[i] instanceof GUIControl && controls[i] != vidc) {
                    append ((Item) controls[i]);
                }
                if (controls[i] instanceof VolumeControl) {
                    vc = (VolumeControl) controls[i];
                }
                if (controls[i] instanceof RateControl) {
                    rc = (RateControl) controls[i];
                }
            }
            status = new StringItem ("Status: ", "");
            status.setLayout (Item.LAYOUT_NEWLINE_AFTER);
            append (status);
            if (vc != null) {
                audioStatus = new StringItem ("", "Volume:");
                audioStatus.setLayout (Item.LAYOUT_NEWLINE_AFTER);
                append (audioStatus);
            }
            time = new StringItem ("", "");
            time.setLayout (Item.LAYOUT_NEWLINE_AFTER);
            append (time);
            player.prefetch ();
            if (videoItem == null)
                addCommand (pauseCommand);
            else {
                Spacer spacer = new Spacer (3, 10);
                spacer.setLayout (Item.LAYOUT_NEWLINE_BEFORE);
                append (spacer);
                append (videoItem);
            }
            Thread t = new Thread (this);
            t.start ();
        }
        catch (Exception me) {
            System.err.println (me);
            close ();
        }
    }

    public void start () {
        if (player == null)
            return;
        try {
            duration = player.getDuration ();
            player.start ();
        }
        catch (Exception ex) {
            System.err.println (ex);
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

    private synchronized void updateStatus () {
        if (player == null)
            return;
        status.setText (
            ((player.getState () == Player.STARTED) ? "Playing, " : "Paused, ") +
                "Rate: " + (currentRate / 1000) + "%\n");
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
        else if (evt == STARTED || evt == STOPPED) {
            updateStatus ();
        }
    }

    private void doSnapshot () {
        new SnapshotThread ().start ();
    }

    class SnapshotThread extends Thread {
        public void run () {
            try {
                byte[] snap = vidc.getSnapshot ("encoding=jpeg");
                if (snap != null) {
                    Image im = Image.createImage (snap, 0, snap.length);
                    ImageItem imi = new ImageItem ("", im, 0, "");
                    append (imi);
                }
            }
            catch (MediaException me) {
                System.err.println (me);
                me.printStackTrace ();
            }
        }
    }

    public synchronized void stopVideoPlayer () {
        // stop & deallocate
        player.deallocate ();
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
            try {
                player.stop ();
            }
            catch (MediaException me) {
                // fall through
            }

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
}
