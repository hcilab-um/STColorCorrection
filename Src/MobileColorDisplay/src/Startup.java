import javax.microedition.midlet.*;
import javax.microedition.lcdui.*;

public class Startup 
	extends MIDlet
{
	/*
	 * Default constructor used by AMS to create an instance 
	 * of our main MIDlet class.
	 */
	public Startup()
	{
	}
	
	/*
	 * startApp() is called by the AMS after it has successfully created
	 * an instance of our MIDlet class. startApp() causes our MIDlet to
	 * go into a "Active" state. 
	 */
	protected void startApp() throws MIDletStateChangeException
	{
		Display display = Display.getDisplay( this );
		//GameScreen extends Canvas which extends Displayable so it can
		// be displayed directly
		display.setCurrent( new GameScreen() );
	}
	
	/*
	* destroyApp() is called by the AMS when the MIDlet is to be destroyed
	*/
	protected void destroyApp( boolean unconditional ) 
		throws MIDletStateChangeException
	{

	}
	
	/*
	* pauseApp() is called by the AMS when the MIDlet should enter a paused 
	* state. This is not a typical "game" pause, but rater an environment pause.
	* The most common example is an incoming phone call on the device, 
	* which will cause the pauseApp() method to be called. This allows
	* us to perform the needed actions within our MIDlet
	*/
	protected void pauseApp()
	{
	}
}
