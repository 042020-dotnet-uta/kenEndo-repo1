using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class RegistrationModel
    {
		public int UserInfoId { get; set; } //PRIMARY KEY

		private string _fName; //first name
		[DisplayName("First Name")]
		[Required(ErrorMessage = "This field is required.")]
		public string fName
		{
			get { return _fName; }
			set { _fName = value; }
		}


		private string _lName; //last name
		[DisplayName("Last Name")]
		[Required(ErrorMessage = "This field is required.")]
		public string lName
		{
			get { return _lName; }
			set { _lName = value; }
		}


		private string _userName; //username
		[DisplayName("User Name")]
		[Required(ErrorMessage = "This field is required.")]
		public string userName
		{
			get { return _userName; }
			set { _userName = value; }
		}
		
		private string _password; //password
		[DisplayName("Password")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "This field is required.")]
		public string password
		{
			get { return _password; }
			set { _password = value; }
		}
		private string _confirmPassword;

		[DisplayName("Confirm Password")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "This field is required.")]
		[Compare("password")]
		public string confirmPassword
		{
			get { return _confirmPassword; }
			set { _confirmPassword = value; }
		}


	}
}
