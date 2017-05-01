package frame;

import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintStream;
import java.net.*;

public class ApplicationFrame extends JFrame 
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private JButton loginButton = new JButton("Login");
	
	private JLabel lblStudentNum = new JLabel("Enter Student Number: ");
	private JTextField txtStudentNum = new JTextField();
	
	private JLabel lblPassword = new JLabel("Enter Password: ");
	private JTextField txtPassword = new JTextField();
	
	private JPanel loginPanel = new JPanel();
	
	public ApplicationFrame()
	{
		setTitle("Login Page");
		setSize(500, 500);
		
		//Login Button Click
		loginButton.addActionListener(new ActionListener()
		{

			@Override
			public void actionPerformed(ActionEvent e) 
			{
				//Send a request to service.
				
				//Determine result.
				
				/////////////////////////////////////////////////////////////
				String studentNumber = txtStudentNum.getText().toString();
				
				//Will do a SHA-2 hash algorithm. Not sending the raw string.
				String password = txtPassword.getText().toString();
				
				try 
				{
					URL url = new URL("http://localhost:55250/Service1.svc/loginUser/" + studentNumber + "/" + password);
			        URLConnection urlc = url.openConnection();
					
			        //PrintStream ps = new PrintStream(urlc.getOutputStream());
			        //ps.close();
			        
					BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlc.getInputStream()));
					
					String line = "";
			        while (bufferedReader.ready()) 
			        {
			        	line = bufferedReader.readLine();
			            System.out.println(line);
			        }
			        
					bufferedReader.close();
				} 
				catch (IOException e1) 
				{
					e1.printStackTrace();
				}
				/////////////////////////////////////////////////////////////
			}
			
		});
		
		loginPanel.setLayout(new GridLayout(5, 1));
		
		loginPanel.add(lblStudentNum);
		loginPanel.add(txtStudentNum);
		
		loginPanel.add(lblPassword);
		loginPanel.add(txtPassword);
		
		loginPanel.add(loginButton);
		
		add(loginPanel);
	}

}
