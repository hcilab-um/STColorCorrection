import javax.microedition.lcdui.*;
 
public class HelloCanvas extends Canvas implements Runnable{
 
     int color=0;
     
    Thread thread = null;
    boolean mySayHello = true;
     
    public void init(){
 
        try{
        	start();
         
        }catch ( Exception e ){
 
            e.printStackTrace();
        }
    }
 
    public void start(){
 
        if( thread == null ){
 
            thread = new Thread( this );
        thread.start();    
        }
    }
 
    public void stop(){
         
        thread = null;
    }
 
    public void run(){
 
 
    }
     
    public int process()
    {
         
        int x = 1;
        int z = 0;
        while(true)
        {
 
        try{
             
            for( z = 1; z <= 255; z++ )
             {
            	x =z;
                Thread.sleep( 100 );
               repaint();
               return x;
             }
                  
             }
        catch ( InterruptedException e ) 
        {
        }
        //repaint();
        
        }
    }
 
     
     
    public void toggleHello(){
         
       // mySayHello = !mySayHello;
    }
     
    public void paint( Graphics g ){
         
       // if ( mySayHello ){
         
            HelloCanvas c = new HelloCanvas();
            int width = getWidth();
            int height = getHeight();
            g.setColor( c.process() , 0, 0 );
            g.fillRect( 0, 0, width, height );
        //}
    }
}