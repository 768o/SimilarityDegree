using MySQL_EC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommend
{
    class Test
    {
        public static List<User> GetAllUser()
        {
            List<User> List_User = new List<User>();
            User user = new User();
            Hashtable item = new Hashtable();
            item.Add(1, 2);
            item.Add(2, 0.5);
            item.Add(4, 1);
            user.id = 1;
            user.Items =item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(1, 4);
            item.Add(2, 2);
            item.Add(3, 1);
            item.Add(4, 2);
            item.Add(5, 0);
            user.id = 2;
            user.Items = item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(1, 1);
            item.Add(3, 2);
            item.Add(4, 1);
            item.Add(5, 0);
            item.Add(6, 1);
            user.id = 3;
            user.Items = item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(1, 2);
            item.Add(4, 2);
            item.Add(5, 3);
            user.id = 4;
            user.Items = item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(3, 2);
            item.Add(4, 2);
            item.Add(5, 0);
            item.Add(6, 1);
            user.id = 5;
            user.Items = item;
            List_User.Add(user);

            return List_User;
        }
        static void Main(string[] args)
        {
            List<User> List_User = GetMovielens();
            User Goal_User = new User { id = 1 };//目标用户id为1

            foreach (User user in List_User)
                if (Goal_User.id == user.id)
                    Goal_User = user;

            Hashtable ht = Goal_User.GetSimilarityDegrees(List_User);
            foreach (DictionaryEntry de in ht) 
                Console.WriteLine("User"+de.Key + ",   " + de.Value);
            Console.WriteLine("Enter Get Recomment");
            Console.ReadKey();

            Hashtable ht1 = Goal_User.GetRecommendItem(List_User);

            //先定义两个一维数组，分别用来存储Key和Value
            int[]keyArray=new int[ht1.Count];
            double[]valueArray=new double[ht1.Count];
            //将HashTable中的Key和Value分别赋给上面两个数组
            //注：有关CopyTo的用法请参考相关帮助文档
            ht1.Keys.CopyTo(keyArray,0);
            ht1.Values.CopyTo(valueArray,0);
            //下面就是对Value进行排序，当然需要按排序结果将Keys的值也作对应的排列 
            //Sort默认是升序排序，如果想用降序排序请在Sort排序后使用Array.Reverse()进行反向排序 
            Array.Sort(valueArray,keyArray);
            Array.Reverse(keyArray);


            int sum = 0;
            foreach (DictionaryEntry de in ht1)
                if (double.Parse(de.Value.ToString()) > 0)
                {
                    sum++;
                    Console.WriteLine("Item" + de.Key + ",   " + de.Value);
                }
            Console.WriteLine("共推荐" + sum +"条数据");
            Console.WriteLine("前xx条数据为");
            int n = 0;
            int[] Rkey = new int[10];
            foreach (int i in keyArray) {
                if (n >= Rkey.Length) break;
                if (double.Parse(ht1[i].ToString())>0) {
                    Console.WriteLine("Item" + i + ",   " + double.Parse(ht1[i].ToString()));
                    Rkey[n] = i;
                    n++;
                }
            }
            IService service = new ServiceImpl();
            DataTable rating = service.SelectToDateTable("YCSJ", null, null);//推测数据
            DataTable yes = service.SelectToDateTable("DBSJ", null, null);//正确数据，101条左右
            int y = 0;
            foreach (int i in Rkey) {
                foreach (DataRow row in yes.Rows) {
                    int iii = int.Parse((row[2].ToString()));
                    if (int.Parse((row[2].ToString())) == i) {
                        y++;
                    }
                }
            }
            double d1 = y;
            double d2 = n;
            Console.WriteLine("推荐" + n + ",   " + "相关" + y);
            Console.WriteLine("准确率" +d1/n + ",   " + "召回率" + d1/101);

            Console.ReadKey();
        }
        public static List<User> GetMovielens() {
            IService service = new ServiceImpl();
            DataTable rating = service.SelectToDateTable("YCSJ", null, "user_id ASC");
            DataTable yes = service.SelectToDateTable("DBSJ", null, "user_id ASC");//正确数据，80条左右
            //string r = service.Select("View1", null, "user_id ASC");
            Hashtable readuser = new Hashtable();
            foreach (DataRow Row in rating.Rows) {
                int user_id = int.Parse(Row[2].ToString());
                if (readuser.ContainsKey(user_id))
                {
                    ((Hashtable)readuser[user_id]).Add(int.Parse(Row[1].ToString()), double.Parse(Row[3].ToString())-3);
                    //((double.Parse(Row[3].ToString()) - 2) > 0) ? (double.Parse(Row[3].ToString()) - 2) : 0
                }
                else
                {
                    Hashtable table = new Hashtable();
                    table.Add(int.Parse(Row[1].ToString()), double.Parse(Row[3].ToString())-3);
                    //((double.Parse(Row[3].ToString()) - 2) > 0) ? (double.Parse(Row[3].ToString()) - 2) : 0
                    readuser.Add(user_id, new Hashtable());
                }
            }
            List<User> list = new List<User>();
            User user;
            foreach (DictionaryEntry de in readuser) {
                user = new User();
                user.id = int.Parse(de.Key.ToString());
                user.Items = ((Hashtable)de.Value);
                list.Add(user);
            }
            return list;
        }
    }
}
