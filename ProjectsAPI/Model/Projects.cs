using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectsAPI.Model
{
    public class Projects
    {
        [JsonPropertyName("Id")]
        public int Id
        {
            get;
            set;
        }       
        [Required]
        [JsonPropertyName("ProjectName")]
        public string ProjectName
        {
            get;
            set;
        } 
        [JsonPropertyName("ClientName")]
        public string ClientName
        {
            get;
            set;
        }
        [JsonPropertyName("projectCost")]
        public double projectCost
        {
            get;
            set;
        }
        [Required]
        [JsonPropertyName("projectManager")]
        public string projectManager
        {
            get;
            set;
        }
        [JsonPropertyName("ratePerHour")]
        public double ratePerHour
        {
            get;
            set;
        }

        [JsonPropertyName("projectUsers")]
        public string projectUsers
        {
            get;
            set;
        }
        [JsonPropertyName("description")]
        [MaxLength(1000)]
        public string description
        {
            get;
            set;
        }
        //[JsonPropertyName("isActive")]
        public Boolean isActive
        {
            get;
            set;
        }
        //[JsonPropertyName("isDeleted")]
        public Boolean isDeleted
        {
            get;
            set;
        }
        //[JsonPropertyName("CreatedDate")]
        public DateTime CreatedDate
        {
            get;
            set;
        }
        //[JsonPropertyName("CreatedBy")]
        public string? CreatedBy
        {
            get;
            set;
        }
        //[JsonPropertyName("UpdatedDate")]
        public DateTime UpdatedDate
        {
            get;
            set;
        }
        //[JsonPropertyName("UpdatedBy")]
        public string? UpdatedBy
        {
            get;
            set;
        }

    }
}
