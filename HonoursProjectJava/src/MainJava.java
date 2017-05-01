import javax.swing.JFrame;

import frame.ApplicationFrame;

public class MainJava 
{
	public static void main(String[] args)
	{
		//Creates application and runs it.
		ApplicationFrame viewRecords = new ApplicationFrame();
		viewRecords.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		viewRecords.setLocation(0,0);
		viewRecords.setVisible(true);
	}
	
}
