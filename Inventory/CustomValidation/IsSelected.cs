using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Inventory.CustomValidation
{
    public class IsSelected:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                if (Convert.ToInt64(value) == 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
           
                    
        }
    }
}