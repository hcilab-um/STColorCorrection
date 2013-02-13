import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Display;
import javax.microedition.lcdui.Graphics;
import javax.microedition.midlet.MIDlet;
import javax.microedition.midlet.MIDletStateChangeException;


public class colorOLED extends MIDlet {
	private Display display;
	private Canvas canvas;

	public colorOLED() {
		display = Display.getDisplay(this);
		canvas = new Canvas() 
		    {
		      protected void paint(Graphics graphics) 
		      {
		        graphics.setColor(0xFF0000);
		        graphics.fillRect(0, 0, getWidth(), getHeight());
		      }
		    };
	}

	protected void destroyApp(boolean unconditional)
			throws MIDletStateChangeException {
		// TODO Auto-generated method stub

	}

	protected void pauseApp() {
		// TODO Auto-generated method stub

	}

	protected void startApp() throws MIDletStateChangeException {
		// TODO Auto-generated method stub
		display.setCurrent(canvas);
	}

}
