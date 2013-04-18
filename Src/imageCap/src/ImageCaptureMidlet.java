import javax.microedition.midlet.*;
import javax.microedition.lcdui.*;
import javax.microedition.media.*;
import javax.microedition.media.Control.*;
import javax.microedition.media.control.VideoControl;


public class ImageCaptureMidlet extends MIDlet {

      Player player;
      VideoControl videoControl;
   

      public void destroyApp(boolean unconditional) {
            notifyDestroyed();
      }
      

	protected void pauseApp() {
		// TODO Auto-generated method stub

	}

	protected void startApp() throws MIDletStateChangeException {
	
         try {
               player = Manager.createPlayer("capture://video");
               player.realize();
               videoControl = (VideoControl) player.getControl("VideoControl");
         } catch (Exception e) {
         }
         // TODO Auto-generated method stub
		Display display = Display.getDisplay( this );
		//GameScreen extends Canvas which extends Displayable so it can
		// be displayed directly
		display.setCurrent( new GameScreen() );
	}
}