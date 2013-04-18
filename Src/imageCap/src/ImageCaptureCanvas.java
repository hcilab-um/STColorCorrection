import javax.microedition.lcdui.*;
import javax.microedition.media.control.VideoControl;
import javax.microedition.media.*;

public class ImageCaptureCanvas extends Canvas {

      ImageCaptureMidlet midlet;
      VideoControl videoControl;
      int width = getWidth();
      int height = getHeight();
      Player player;
      SnapShotCanvas snap;
      private Display display;

      public ImageCaptureCanvas(ImageCaptureMidlet midlet, VideoControl videoControl) throws MediaException {
            this.midlet = midlet;
            this.videoControl = videoControl;
            this.display = Display.getDisplay(midlet);
            videoControl.initDisplayMode(VideoControl.USE_DIRECT_VIDEO, this);
            try {
                  videoControl.setDisplayLocation(2, 2);
                  videoControl.setDisplaySize(width - 4, height - 4);
            } catch (MediaException me) {
                  try {
                        videoControl.setDisplayFullScreen(true);
                  } catch (MediaException me2) {
                  }
            }
            videoControl.setVisible(true);
           
            Thread t = new Thread(new Runnable() {
            	 public void run() {
            	 try {
            	 player.start();
            	 } catch (MediaException ex) {
            	 ex.printStackTrace();
            	 }
            	 }
            	 });
      }

      public void paint(Graphics g) {
      }

      //protected void keyPressed(int keyCode) 
      public void run(){
           // switch (getGameAction(keyCode)) 
    	  {
                 // case FIRE:
                        Thread t = new Thread() {
                              public void run() {
                                    try {
                                          byte[] raw = videoControl.getSnapshot(null);
                                          Image image = Image.createImage(raw, 0, raw.length);
                                          snap = new SnapShotCanvas(image);
                                          display.setCurrent(snap);
                                    } catch (Exception e) {
                                    }
                              }
                        };
                        t.start();
            }
      }
}