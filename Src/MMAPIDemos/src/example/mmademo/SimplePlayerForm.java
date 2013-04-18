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
import javax.microedition.media.Control;
import javax.microedition.media.MediaException;
import javax.microedition.media.control.GUIControl;
import javax.microedition.media.control.VideoControl;
import java.io.InputStream;


/**
 * MMAPI player main window for media files, implemented as a Form
 *
 * @version 1.4
 */
public class SimplePlayerForm extends Form
    implements SimplePlayerGUI.Parent,
    Utils.ContentHandler,
    Utils.Interruptable,
    Runnable {

    private SimplePlayerGUI gui; // default: null
    private Utils.BreadCrumbTrail parent;

    private ImageItem iiLogo;
    private StringItem siFileTitle;
    private StringItem siTime;
    private StringItem siRate;
    private StringItem siKaraoke;
    private StringItem siFeedback;
    private StringItem siStatus;

    private boolean karaokeShowing;

    private static int maxKaraokeLines = 2;

    private static void debugOut (String s) {
        Utils.debugOut ("SimplePlayerForm: " + s);
    }

    public SimplePlayerForm (String title, Utils.BreadCrumbTrail parent) {
        this (title, null, parent);
    }

    public SimplePlayerForm (String title, SimplePlayerGUI spg, Utils.BreadCrumbTrail parent) {
        super (title);
        this.parent = parent;
        this.gui = spg;
        siFileTitle = new StringItem ("", "");
        siTime = new StringItem ("", "");
        siRate = new StringItem ("", "");
        siKaraoke = new StringItem ("", "");
        siFeedback = new StringItem ("", "");
        siStatus = new StringItem ("", "");
        debugOut ("constructor finished");
    }

    private void appendNewLine (Item item) {
        insertNewLine (size (), item);
    }

    private void insertNewLine (int pos, Item item) {
        insert (pos, item);

        Spacer spacer = new Spacer (3, 10);
        spacer.setLayout (Item.LAYOUT_NEWLINE_BEFORE);
        if (pos < 8) {
            insert (pos++, spacer);
        }
    }

    private void setUpItems () {
        // first delete all items
        for (int i = size () - 1; i >= 0; i--) {
            delete (i);
        }
        karaokeShowing = false;
        getGUI ();
        if (!gui.hasGUIControls ()) {
            makeImageItem ();
        }
        appendNewLine (siFileTitle);
        appendNewLine (siTime);
        if (gui.hasRateControl () || gui.hasTempoControl ()) {
            appendNewLine (siRate);
        }
        if (gui.hasGUIControls ()) {
            Control[] controls = gui.getControls ();
            if (controls != null) {
                for (int i = 0; i < controls.length; i++) {
                    Control ctrl = controls[i];
                    if (ctrl instanceof GUIControl) {
                        Object guiItem =
                            ((GUIControl) ctrl).initDisplayMode (GUIControl.USE_GUI_PRIMITIVE, null);
                        if (guiItem instanceof Item) {
                            append ((Item) guiItem);
                            if (ctrl instanceof VideoControl) {
                                try {
                                    ((VideoControl) ctrl).setDisplayFullScreen (false);
                                }
                                catch (MediaException me) {
                                    Utils.debugOut (me);
                                }
                            }
                        }
                    }
                }
            }
        }
        appendNewLine (siFeedback);
        appendNewLine (siStatus);
    }

    private void makeImageItem () {
        if (gui != null && !gui.isFullScreen ()) {
            if (iiLogo == null) {
                Image logo = gui.getLogo ();
                if (logo != null) {
                    iiLogo = new ImageItem (
                        "", logo,
                        ImageItem.LAYOUT_CENTER
                            | ImageItem.LAYOUT_NEWLINE_BEFORE
                            | ImageItem.LAYOUT_NEWLINE_AFTER, "MMAPI logo");
                }
            }
            if (iiLogo != null) {
                insert (0, iiLogo);
            }
        }
    }

    ////////////////////////////// interface Utils.BreadCrumbTrail /////////////////

    public Displayable go (Displayable d) {
        return parent.go (d);
    }

    public Displayable goBack () {
        return parent.goBack ();
    }

    public Displayable replaceCurrent (Displayable d) {
        return parent.replaceCurrent (d);
    }

    public Displayable getCurrentDisplayable () {
        return parent.getCurrentDisplayable ();
    }

    /////////////////////////// interface SimplePlayerGUI.Parent //////////////////

    public Utils.BreadCrumbTrail getParent () {
        return parent;
    }

    // called after the media is prefetched
    public void setupDisplay () {
        setUpItems ();
    }

    public void setStatus (String s) {
        siStatus.setText (s);
    }

    public void setFeedback (String s) {
        siFeedback.setText (s);
    }

    public void setFileTitle (String s) {
        siFileTitle.setText (s);
    }

    public void updateKaraoke () {
        int[] karaokeParams = new int[4];
        String[] lines = gui.getKaraokeStr (karaokeParams);
        int currLine = karaokeParams[SimplePlayerGUI.KARAOKE_LINE];
        int lineCount = karaokeParams[SimplePlayerGUI.KARAOKE_LINE_COUNT];
        int syllLen = karaokeParams[SimplePlayerGUI.KARAOKE_SYLLABLE_LENGTH];
        int currLinePos = karaokeParams[SimplePlayerGUI.KARAOKE_LINE_INDEX];

        int thisLine = 0;
        if (lineCount > maxKaraokeLines) {
            thisLine = currLine - 1;
            if (thisLine < 0) {
                thisLine = 0;
            }
            if (thisLine + maxKaraokeLines > lineCount) {
                thisLine = lineCount - maxKaraokeLines;
            }
            else if (lineCount - thisLine > maxKaraokeLines) {
                lineCount = thisLine + maxKaraokeLines;
            }
        }
        String text = "";
        for (; thisLine < lineCount; thisLine++) {
            text += lines[thisLine] + "\n";
        }

        siKaraoke.setText (text);
        if (!karaokeShowing && !gui.isFullScreen ()) {
            // insert karaoke item before feedback line
            for (int i = size () - 1; i >= 0; i--) {
                if (get (i) == siFeedback) {
                    if (i > 0 && (get (i - 1) instanceof ImageItem)) {
                        i--;
                    }
                    insertNewLine (i, siKaraoke);
                    break;
                }
            }
            // do not try to visualize siKaraoke again
            karaokeShowing = true;
        }

    }

    public void updateTime () {
        if (gui != null) {
            siTime.setText (gui.getMediaTimeStr ());
        }
    }

    public void updateRate () {
        if (getGUI ().hasTempoControl ()) {
            siRate.setText (gui.getTempoStr ());
        }
        else {
            siRate.setText (gui.getRateStr ());
        }
    }

    public void updateDisplay () {
    }

    public void fullScreen (boolean value) {
    }

    //////////////////////////////// interface Utils.ContentHandler ///////////////////////////////

    public synchronized void close () {
        if (gui != null) {
            gui.closePlayer ();
            gui = null;
        }
    }

    public boolean canHandle (String url) {
        return true;
    }

    public void handle (String name, String url) {
        Utils.debugOut ("SimplePlayerForm: handle " + url);
        getGUI ().setParent (this);
        gui.setSong (name, url);
        doHandle ();
    }

    public void handle (String name, InputStream is, String contentType) {
        getGUI ().setParent (this);
        gui.setSong (name, is, contentType);
        doHandle ();
    }

    // ///////////////////////// interface Utils.ContentHandler //////////////// //

    private synchronized SimplePlayerGUI getGUI () {
        if (gui == null) {
            debugOut ("create GUI");
            gui = new SimplePlayerGUI ();
            gui.initialize (getTitle (), this);
            gui.setTimerInterval (500);
            makeImageItem ();
        }
        return gui;
    }


    private void doHandle () {
        debugOut ("doHandle");
        setUpItems ();
        // IMPL NOTE:
        // I want to display the player first, and THEN start prefetching.
        // the only way I was able to achieve this was by creating a new thread.
        new Thread (this).start ();
    }

    public void run () {
        gui.startPlayer ();
    }

    // /////////////////////////////////// Interface Utils.Interruptable ////////////////// //

    /**
     * Called in response to a request to pause the MIDlet.
     * This implementation will just call the same
     * method in the GUI implementation.
     */
    public synchronized void pauseApp () {
        if (gui != null) {
            gui.pauseApp ();
        }
    }


    /**
     * Called when a MIDlet is asked to resume operations
     * after a call to pauseApp(). This method is only
     * called after pauseApp(), so it is different from
     * MIDlet's startApp().
     * <p/>
     * This implementation will just call the same
     * method in the GUI implementation.
     */
    public synchronized void resumeApp () {
        if (gui != null) {
            gui.resumeApp ();
        }
    }


    // for debugging
    public String toString () {
        return "SimplePlayerForm";
    }

}
