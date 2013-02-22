import javax.bluetooth.DeviceClass;
import javax.bluetooth.DiscoveryAgent;
import javax.bluetooth.DiscoveryListener;
import javax.bluetooth.LocalDevice;
import javax.bluetooth.RemoteDevice;
import javax.bluetooth.ServiceRecord;
import javax.bluetooth.UUID;
import javax.microedition.lcdui.*;
import javax.microedition.midlet.MIDlet;
import javax.microedition.midlet.MIDletStateChangeException;
import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Display;
import javax.microedition.lcdui.Graphics;

public class colorOLED extends MIDlet implements CommandListener   
{
	java.util.Vector devices,services;
	LocalDevice local;
	DiscoveryAgent agent;
	int currentDevice = 0;
	
	String address="000272334418";
	
	//RemoteDevice device= new RemoteDevice(address);
	
	
	  
	//exit the menu
	private final Command EXIT_CMD = new Command ("Exit", Command.EXIT, 2);
	
	//start a connection
	 private final Command OK_CMD = new Command ("Ok", Command.SCREEN, 1);
	 
	 //Cancels the device/services discovering.
	 private final Command SCR_SEARCH_CANCEL_CMD = new Command ("Back", Command.BACK, 2);
	 
	 //in form of a list- with just connect for this example
	 private static final String[] elements = {"Connect","exit"};
	
	 private final List menu = new List ("Bluetooth Demo", List.IMPLICIT, elements, null);
	 
	private Display display;
	private Canvas canvas;
	
    /**
     * value is true after selecting connect
     */
    private boolean isInit = false;
	
	public colorOLED() 
	{
		
		menu.addCommand (EXIT_CMD);
	    menu.addCommand (OK_CMD);
	    menu.setCommandListener (this);
	    
	    display = Display.getDisplay(this);
		canvas = new Canvas() 
		    {
			// this string should get the value from c# 
			String hexcolor="FF0000";
			//converting the hex string into int 
	 		int tempcolor = (Integer.parseInt(hexcolor, 16)+0xFF000000);; 
			
		      protected void paint(Graphics graphics) 
		      {  	 
		    	// display that hex color
		        graphics.setColor(tempcolor);
		        graphics.fillRect(0, 0, getWidth()/2, getHeight());
		        graphics.setColor(0x00FF00);
		        graphics.fillRect((getWidth()/2), 0, getWidth(), getHeight());
		      }
		    };
	}

	protected void destroyApp(boolean unconditional)
	{
		
		// TODO Auto-generated method stub

	}

	protected void pauseApp() {
		// TODO Auto-generated method stub

	}

	protected void startApp() throws MIDletStateChangeException 
	{
		Display.getDisplay (this).setCurrent(menu);
		// TODO Auto-generated method stub
		 if (isInit==true) 
		 {
			Display.getDisplay (this).setCurrent(canvas);
	     }
		
	}
	
	public void commandAction (Command c, Displayable d)
	{
        if (c == EXIT_CMD) 
        {
            destroyApp (true);
            notifyDestroyed ();

            return;
        }
        
        else if (menu.getSelectedIndex()==0) 
        {
	        
	        	Form f = new Form ("Searching...");
	        	 f.addCommand (SCR_SEARCH_CANCEL_CMD);
	        	 f.setCommandListener (this);
	        	 f.append (
	                     new Gauge (
	                         "Searching images...", false, Gauge.INDEFINITE,
	                         Gauge.CONTINUOUS_RUNNING));
	        	
	        	 Display.getDisplay (this).setCurrent(f);
	        		        	
	        	 FindDevices();
	        	 isInit = true;
	     }
        
        
        else if (c == SCR_SEARCH_CANCEL_CMD) {
        	Display.getDisplay (this).setCurrent(menu);
        }
        
        else if (menu.getSelectedIndex()==0) 
        {
        	destroyApp (true);
            notifyDestroyed ();

            return;
        }
    	
        
    }
	
	
	public void FindDevices()
	{			
	
	    try
	    {
	        devices              = new java.util.Vector();
	        LocalDevice local    = LocalDevice.getLocalDevice();
	        DiscoveryAgent agent = local.getDiscoveryAgent();
	       
	       agent.startInquiry(DiscoveryAgent.GIAC,(DiscoveryListener) this);
	    }catch(Exception e){this.do_alert("Erron in initiating search" , 4000);}
	 }
	
	public void FindServices(RemoteDevice device)
	{
	    try
	    {
	        UUID[] uuids  = new UUID[1];
	        uuids[0] = new UUID("a0000000-a000-a000-a000-a00000000000",false);    //The UUID of the  service
	        local = LocalDevice.getLocalDevice();
	        agent = local.getDiscoveryAgent();
	        agent.searchServices(null,uuids,device,(DiscoveryListener) this);            
	    }catch(Exception e){this.do_alert("Erron in initiating search" , 4000);}
	}
	
	public void deviceDiscovered(RemoteDevice remoteDevice,DeviceClass deviceClass)
	{
	    devices.addElement(remoteDevice);
	}
	
	public void servicesDiscovered(int transID,ServiceRecord[] serviceRecord) {
	    for (int x = 0; x < serviceRecord.length; x++ )
	        services.addElement(serviceRecord[x]);
	    try{
	       // dev_list.append(((RemoteDevice)devices.elementAt(currentDevice)).
	         //                                   getFriendlyName(false),null);
	    }catch(Exception e){this.do_alert("Erron in initiating search" , 4000);}
	}
	public void inquiryCompleted(int param){
	    switch (param) {
	        case DiscoveryListener.INQUIRY_COMPLETED:    //Inquiry completed normally
	            if (devices.size() > 0){                 //Atleast one device has been found
	                services = new java.util.Vector();
	                this.FindServices((RemoteDevice)
	                         devices.elementAt(0));     //Check if the first device offers the service
	            }else
	                do_alert("No device found in range",4000);
	        break;
	        case DiscoveryListener.INQUIRY_ERROR:       // Error during inquiry
	            this.do_alert("Inqury error" , 4000);
	        break;
	        case DiscoveryListener.INQUIRY_TERMINATED:  // Inquiry terminated by agent.cancelInquiry()
	             this.do_alert("Inqury Canceled" , 4000);
	        break;
	       }
	}
	    
	public void serviceSearchCompleted(int transID, int respCode) {
	    switch(respCode) {
	        case DiscoveryListener.SERVICE_SEARCH_COMPLETED:
	            if(currentDevice == devices.size() -1){ //all devices have been searched
	                if(services.size() > 0){
	                   // display.setCurrent(dev_list);
	                }else
	                    do_alert("The service was not found",4000);
	            }else{                               //search next device
	                currentDevice++;
	                this.FindServices((RemoteDevice)devices.elementAt(currentDevice));
	            }
	        break;
	        case DiscoveryListener.SERVICE_SEARCH_DEVICE_NOT_REACHABLE:
	             this.do_alert("Device not Reachable" , 4000);
	        break;
	        case DiscoveryListener.SERVICE_SEARCH_ERROR:
	             this.do_alert("Service serch error" , 4000);
	        break;
	        case DiscoveryListener.SERVICE_SEARCH_NO_RECORDS:
	            this.do_alert("No records returned" , 4000);
	        break;
	        case DiscoveryListener.SERVICE_SEARCH_TERMINATED:
	            this.do_alert("Inqury Cancled" , 4000);
	        break;
	     }
	}
	
	public void do_alert(String msg,int time_out){
	    if (display.getCurrent() instanceof Alert ){
	        ((Alert)display.getCurrent()).setString(msg);
	        ((Alert)display.getCurrent()).setTimeout(time_out);
	    }else{
	        Alert alert = new Alert("Bluetooth");
	        alert.setString(msg);
	        alert.setTimeout(time_out);
	        display.setCurrent(alert);
	    }
	}
	

}
