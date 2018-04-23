using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Models {
    public static class Helper {
        public static Dictionary<object, byte[]> Files {
            get {
                if (HttpContext.Current.Session["files"] == null)
                    HttpContext.Current.Session["files"] = new Dictionary<object, byte[]>();
                return HttpContext.Current.Session["files"] as Dictionary<object, byte[]>;
            }
            set {
                HttpContext.Current.Session["files"] = value;
            }
        }
        public static IEnumerable<TestModel> GetData() {
            return Enumerable.Range(0, 10).Select(i => new TestModel() {
                ID = i,
                PersonName = "Name " + i,
                FileName = ""
            });
        }
    }

    

    public class TestModel {
        public int ID { get; set; }
        public string PersonName { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
    
}