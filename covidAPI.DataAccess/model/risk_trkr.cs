using System;
namespace covidAPI.model
{
    public class risk_trkr
    {
        public int u_id { get; set; }
        public string s_r_id { get; set; }
        public bool? travel_hist { get; set; }
        public bool? contact { get; set; }
        public int? risk { get; set; }
        
    }
}
