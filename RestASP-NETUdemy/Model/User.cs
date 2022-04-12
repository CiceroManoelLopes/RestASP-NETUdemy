using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestASP_NETUdemy.Model
{
    [Table("users")]
    public class Users
    {
        [Key]        
        public long id { get; set; }       
        public string user_name { get; set; }       
        public string full_name { get; set; }       
        public string password { get; set; }       
        public string refresh_token { get; set; }       
        public DateTime refresh_token_expiry_time { get; set; }

    }
}
