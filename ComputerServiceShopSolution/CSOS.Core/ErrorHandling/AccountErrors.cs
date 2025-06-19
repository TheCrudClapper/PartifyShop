using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.ErrorHandling
{
    public class AccountErrors
    {

        public static readonly Error AccountNotFound = new Error(
            "User.AccountNotFound", "Account of given id not found");

        public static readonly Error WrongPassword = new Error(
            "User.WrongPassword", "You've given wrong password");

        public static readonly Error PasswordsDoestMatch = new Error(
           "User.PasswordsDoestMatch", "New and Confirmation passwords doesnt match");

        public static readonly Error PasswordChangeFailed = new Error(
           "User.PasswordChangeFailed", "Failed to change passwords");

    }
}
