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

import javax.microedition.io.Connector;
import javax.microedition.media.control.ToneControl;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Vector;


/**
 * Converts ring tone files to the MMAPI tone player format.
 * Those file as can be downloaded from the Internet
 *
 * @version 1.4
 */
public class RingToneConverter {
    // internal parser data
    private byte[] inputData;
    private int readPos;
    private Vector notes;
    private Vector lengths;
    private char lastSeparator;
    private int tempo = 80; // in beats per second

    // output data
    private String name;
    private byte[] sequence;

    /**
     * Tries to convert the passed file. The entire contents are read
     * into memory and then different parsers are tried.
     *
     * @throws IOException - on read error
     * @throws Exception   - if <code>url</code> does not contain a valid ring tone text file
     */
    public RingToneConverter (String url) throws Exception {
        this (url, URL2Name (url));
    }

    public RingToneConverter (String url, String name) throws Exception {
        this (Connector.openInputStream (url), name);
    }

    public RingToneConverter (InputStream is, String name) throws Exception {
        this (readInputStream (is), name);
    }

    public RingToneConverter (byte[] data, String name) throws Exception {
        this.inputData = data;
        this.name = name;
        notes = new Vector ();
        lengths = new Vector ();
        boolean success = parseRTTTL ();
        if (!success) {
            throw new Exception ("Not a supported ringtone text file");
        }
        if (tempo < 20 || tempo > 508) {
            throw new Exception ("tempo is out of range");
        }
        inputData = null;
        System.gc ();
        sequence = new byte[notes.size () * 2 + 4];
        sequence[0] = ToneControl.VERSION;
        sequence[1] = 1;
        sequence[2] = ToneControl.TEMPO;
        sequence[3] = (byte) ((tempo >> 2) & 0x7f);
        for (int i = 0; i < notes.size (); i++) {
            sequence[2 * i + 4] = (byte) (((Integer) notes.elementAt (i)).intValue () & 0xff);
            sequence[2 * i + 5] = (byte) (((Integer) lengths.elementAt (i)).intValue () & 0x7f);
        }
        notes = null;
        lengths = null;
        System.gc ();
    }

    public String getName () {
        return name;
    }

    public byte[] getSequence () {
        return sequence;
    }

    /**
     * Dump the sequence as hexadecimal
     * numbers to standard out. This
     * can be used to create .jts files
     * from RTTTL files.
     */
    public void dumpSequence () {
        String[] hexChars = {
            "0", "1", "2", "3",
            "4", "5", "6", "7",
            "8", "9", "A", "B",
            "C", "D", "E", "F"};
        for (int i = 0; i < sequence.length; i++) {
            System.out.print (
                hexChars[(sequence[i] & 0xF0) >> 4]
                    + hexChars[sequence[i] & 0xF] + " ");
            if (i % 8 == 7) System.out.println ("");
        }
        System.out.println ("");
    }

    // note: strings must be sorted in descending order of their length
    private static final String[] durationStrings = {"16", "32", "1", "2", "4", "8"};
    private static final int[] durationValues = {16, 32, 1, 2, 4, 8};
    private static final String[] noteStrings =
        {"C#", "D#", "F#", "G#", "A#", "C", "D", "E", "F", "G", "A", "H", "B"};
    private static final int[] noteValues =
        {1, 3, 6, 8, 10, 0, 2, 4, 5, 7, 9, 11, 11}; // H (German) == B (English)
    private static final String[] scaleStrings = {"4", "5", "6", "7", "8"};
    private static final int[] scaleValues = {4, 5, 6, 7, 8};

    /**
     * Parse a ringtone sequence in RTTTL
     * (Ringing Tones text transfer language) format.
     * <p/>
     * Example:
     * Entertainer:d=4, o=5, b=140:8d, 8d#, 8e, c6, 8e, c6, 8e,
     * 2c.6, 8c6, 8d6, 8d#6, 8e6, 8c6, 8d6, e6, 8b, d6, 2c6, p,
     * 8d, 8d#, 8e, c6, 8e, c6, 8e, 2c.6, 8p, 8a, 8g, 8f#, 8a,
     * 8c6, e6, 8d6, 8c6, 8a, 2d6
     *
     * @return true if parsing was successful
     */
    private boolean parseRTTTL () {
        boolean result = true;
        try {
            // default tempo is 63
            tempo = 63;
            // default duration is a quarter note
            int defDuration = 4;
            // default octave is 6
            int defScale = 6;

            // start with Name, followed by colon :
            String songName = readString (":", false, false);
            if (songName.length () > 0) {
                name = songName;
            }
            // read defaults
            do {
                String def = readString (",:", true, true);
                if (def != "") {
                    if (def.startsWith ("D=")) {
                        defDuration = Integer.parseInt (def.substring (2));
                    }
                    else if (def.startsWith ("O=")) {
                        defScale = Integer.parseInt (def.substring (2));
                    }
                    else if (def.startsWith ("B=")) {
                        tempo = Integer.parseInt (def.substring (2));
                    }
                    else {
                        throw new Exception ("Unknown default \"" + def + "\"");
                    }
                }
                else {
                    if (lastSeparator != ':') {
                        throw new Exception ("':' excepted");
                    }
                    break;
                }
            }
            while (lastSeparator == ',');

            // read note commands
            StringBuffer noteCommand = new StringBuffer ();
            while (lastSeparator != 'E') {
                noteCommand.setLength (0);
                noteCommand.append (readString (",", true, true));
                if (noteCommand.length () == 0) {
                    break;
                }
                // get duration
                int duration = tableLookup (noteCommand, durationStrings, durationValues, defDuration);

                // get note
                int note = tableLookup (noteCommand, noteStrings, noteValues, -1);

                // dotted duration ?
                int dotCount = 0;
                // dot may appear before or after scale
                if (noteCommand.length () > 0 && noteCommand.charAt (0) == '.') {
                    dotCount = 1;
                    noteCommand.deleteCharAt (0);
                }
                if (note >= 0) {
                    // octave
                    int scale = tableLookup (noteCommand, scaleStrings, scaleValues, defScale);
                    note = ToneControl.C4 + ((scale - 4) * 12) + note;
                }
                else {
                    // pause ?
                    if (noteCommand.charAt (0) == 'P') {
                        note = ToneControl.SILENCE;
                        noteCommand.deleteCharAt (0);
                    }
                    else {
                        throw new Exception ("unexpected note command: '" + noteCommand.toString () + "'");
                    }
                }
                // dot may appear before or after scale
                if (noteCommand.length () > 0 && noteCommand.charAt (0) == '.') {
                    dotCount = 1;
                    noteCommand.deleteCharAt (0);
                }
                if (noteCommand.length () > 0) {
                    throw new Exception ("unexpected note command: '" + noteCommand.toString () + "'");
                }
                addNote (note, duration, dotCount);
            }
            Utils.debugOut ("RingToneConverter: read " + notes.size () + " notes successfully.");
        }
        catch (Exception e) {
            Utils.debugOut (e);
            result = false;
        }
        return result;
    }

    // utility methods

    private String readString (String separators, boolean stripWhiteSpace, boolean toUpperCase) {
        int start = readPos;
        lastSeparator = 'E'; // end of file
        boolean hasWhiteSpace = false;
        while (lastSeparator == 'E' && readPos < inputData.length) {
            char input = (char) inputData[readPos++];
            if (input <= 32) {
                hasWhiteSpace = true;
            }
            for (int i = 0; i < separators.length (); i++) {
                if (input == separators.charAt (i)) {
                    // separator found
                    lastSeparator = input;
                    break;
                }
            }
        }
        int end = readPos - 1;
        if (lastSeparator != 'E') {
            // don't return separator
            end--;
        }
        String result = "";
        if (start <= end) {
            result = new String (inputData, start, end - start + 1);
            if (stripWhiteSpace && hasWhiteSpace) {
                // trim result
                StringBuffer sbResult = new StringBuffer (result);
                int i = 0;
                while (i < sbResult.length ()) {

                    if (sbResult.charAt (i) <= 32) {
                        sbResult.deleteCharAt (i);
                    }
                    else {
                        i++;
                    }
                }
                result = sbResult.toString ();
            }
            if (toUpperCase) {
                result = result.toUpperCase ();
            }
        }
        Utils.debugOut ("Returning '" + result + "'  with lastSep='" + lastSeparator + "'");
        return result;
    }

    private static int tableLookup (StringBuffer command, String[] strings, int[] values, int defValue) {
        String sCmd = command.toString ();
        int result = defValue;
        for (int i = 0; i < strings.length; i++) {
            if (sCmd.startsWith (strings[i])) {
                command.delete (0, strings[i].length ());
                result = values[i];
                break;
            }
        }
        return result;
    }

    /**
     * add a note to the <code>notes</code> and <code>lengths</code> Vectors.
     *
     * @param note     - 0-128, as defined in ToneControl
     * @param duration - the divider of a full note. E.g. 4 stands for a quarter note
     * @param dotCount - if 1, then the duration is increased by half its length, if 2 by 3/4 of its length, etc.
     */
    private void addNote (int note, int duration, int dotCount) {
        // int length = (60000 * 4) /(duration * tempo);
        int length = 64 / duration;
        int add = 0;
        int factor = 2;
        for (; dotCount > 0; dotCount--) {
            add += length / factor;
            factor *= 2;
        }
        length += add;
        if (length > 127)
            length = 127;
        notes.addElement (new Integer (note));
        lengths.addElement (new Integer (length));
    }

    private static String URL2Name (String url) {
        int lastSlash = url.lastIndexOf ('/');
        if (lastSlash == -1 || lastSlash == url.length () - 1) {
            lastSlash = url.lastIndexOf (':');
        }
        return url.substring (lastSlash + 1);
    }

    private static byte[] readInputStream (InputStream is) throws IOException {
        ByteArrayOutputStream baos = new ByteArrayOutputStream ();
        byte[] buffer = new byte[128];
        while (true) {
            int read = is.read (buffer);
            if (read < 0) {
                break;
            }
            baos.write (buffer, 0, read);
        }
        is.close ();
        byte[] data = baos.toByteArray ();
        System.gc ();
        return data;
    }
}
