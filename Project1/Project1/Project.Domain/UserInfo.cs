using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain
{
	/// <summary>
	/// Model for user information
	/// </summary>
	public class UserInfo
	{
		public int UserInfoId { get; set; } //PRIMARY KEY

		private string _fName; //first name

		public string fName
		{
			get { return _fName; }
			set { _fName = value; }
		}

		private string _lName; //last name

		public string lName
		{
			get { return _lName; }
			set { _lName = value; }
		}


		private string _userName; //username

		public string userName
		{
			get { return _userName; }
			set { _userName = value; }
		}


		private string _password; //password

		public string password
		{
			get { return _password; }
			set { _password = value; }
		}

		public UserInfo() { }

		public UserInfo(string fname, string lname, string userName, string password) // constructor to store user information when instantiated
		{
			this.fName = fname;
			this.lName = lname;
			this.userName = userName;
			this.password = password;
		}

	}
}
