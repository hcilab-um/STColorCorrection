import javax.microedition.lcdui.*;
import java.util.*;

public class GameScreen 	extends Canvas	implements Runnable
{
	Random generator;
	int colour;
	//Default constructor for our GameScreen class.
	public GameScreen()
	{
		//set up the games random number generator
		generator = new Random( System.currentTimeMillis() );
		//create a new Thread on this Runnable and start it immediately
		new Thread( this ).start();		
	}
	
	/*
	 * run() method defined in the Runnable interface, called by the 
	 * Virtual machine when a Thread is started.
	 */
	public void run()
	{
		while( true )
		{
			//set wanted loop delay to 15th of a second
			int loopDelay = 1000 / 15;
			//get the time at the start of the loop
			long loopStartTime = System.currentTimeMillis();
			//call our tick() fucntion which will be our games heartbeat
			tick();
			//get time at end of loop
			long loopEndTime  = System.currentTimeMillis();
			//caluclate the difference in time from start til end of loop
			int loopTime = (int)(loopEndTime - loopStartTime); 
			//if the difference is less than what we want
			if( loopTime < loopDelay )
			{
				try
				{
					//then sleep for the time needed to fullfill our wanted rate
					Thread.sleep( loopDelay - loopTime );
				}
				catch( Exception e )
				{
				}
				
			}
		}
	}
	
	/*
	 * Our games main loop, called at a fixed rate by our game Thread
	 */
	public void tick()
	{
		//get a random number within the RRGGBB colour range
		colour = generator.nextInt() & 0xFFFFFF; 
		
		//schedule a repaint of the Canvas 
		repaint();
		//forces any pending repaints to be serviced, and blocks until 
		//paint() has returned
		serviceRepaints();
	}
	
	/* 
	 * called when the Canvas is to be painted
	 */
	protected void paint( Graphics g )
	{
		//set the current color of the Graphics contect to the specified RRGGBB colour
		g.setColor( colour );
		//draw a filled rectangle at x,y coordinates 0, 0 with a width
		// and height equal to that of the Canvas itself
		g.fillRect( 0, 0, this.getWidth(), this.getHeight() );		
	}
	
	/* 
	* called when a key is pressed and this Canvas is the
	* current Displayable 
	*/
	protected void keyPressed( int keyCode )
	{
		//get the game action from the passed keyCode
		int gameAction = getGameAction( keyCode );
		switch( gameAction )
		{
			case LEFT:
			//set current colour to red
			colour = 0xFF0000; 
			break;
			
			case RIGHT:
			//set current colour to green
			colour = 0x00FF00;
			break;
			
			case UP:
			//set current colour to black
			colour = 0x000000;
			break;
			
			case DOWN:
			//set current colour to white
			colour = 0xFFFFFF;
			break;
			
			case FIRE:
			//set current colour to blue
			colour = 0x0000FF;
			break;
		}
	}

	/* 
	* called when a key is released  and this Canvas is the
	* current Displayable 
	*/
	protected void keyReleased( int keyCode )
	{
	}
}
