import javax.microedition.midlet.*;
import javax.microedition.lcdui.*;
 
    public class colorOLED extends MIDlet implements CommandListener
    {
         
        HelloCanvas myCanvas;
        private Command exitCommand = new Command( "Exit", Command.EXIT, 99 );
        private Command toggleCommand = new Command( "Toggle_Msg", Command.SCREEN, 1 );
         
        public colorOLED(){
             
            myCanvas = new HelloCanvas();
            myCanvas.addCommand( exitCommand );
            myCanvas.addCommand( toggleCommand );
            myCanvas.setCommandListener( this );
    }
         
        public void startApp() throws MIDletStateChangeException
        {
             
            Display.getDisplay( this ).setCurrent( myCanvas );
            myCanvas.repaint();
        }
         
        public void destroyApp( boolean unconditional ) throws MIDletStateChangeException
        {
        }
         
        public void pauseApp()
        {
        }
         
        public void commandAction( Command c, Displayable s )
        {
             
            if( c == toggleCommand )
            {
                 
                myCanvas.toggleHello();
            }
            else if( c == exitCommand ){
                 
               try
               {
                    destroyApp( false );
                    notifyDestroyed();
                }
                catch ( MIDletStateChangeException ex )
                {
                }
            }
        }
}