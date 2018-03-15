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
        static User Goal_User = new User { id = 1 };//设置目标用户id为1
        static void Main(string[] args)
        {
            List<User> List_User = GetMovielens();//获得movielens用户评分数据

            Console.WriteLine("一共有用户" + List_User.Count + "，目标用户为" + Goal_User.id);

            foreach (User user in List_User)
                if (Goal_User.id == user.id)
                    Goal_User = user;//绑定数据集中的该用户

            int K =5;
            for (int i= 1;i<30;i++) {
                GetResult(List_User,K*i);//不同的K的结果
            }
            Console.ReadKey();
        }
        private static void GetResult(List<User> List_User,int K) {
            List<User> List_User_K = Goal_User.GetSimilarityDegrees(List_User, K);//获得用户相似度表,取前K名,K=20
            Console.WriteLine("K=" + K);
            //Console.ReadKey();

            Hashtable item = Goal_User.GetRecommendItem(List_User_K);//K名用户产生的推荐列表
            int[] ycwzlb = new int[item.Count];//预测为正的item的id列表
            int[] ycwflb = new int[item.Count];//预测为负的item的id列表

            int tp_sum = 0;
            int f_sum = 0;
            foreach (DictionaryEntry de in item)
            {
                if (double.Parse(de.Value.ToString()) > 0)//预测用户会喜欢的
                {
                    ycwzlb[tp_sum] = int.Parse(de.Key.ToString());
                    tp_sum++;
                }
                else
                {
                    ycwflb[f_sum] = int.Parse(de.Key.ToString());
                    f_sum++;
                }
            }
            //Console.WriteLine("预测为正item列表");
            //foreach (int i in ycwzlb)
            //{
            //    if (i != 0)
            //        Console.Write(i + ",");
            //}
            //Console.WriteLine();
            //Console.WriteLine("预测为负item列表");
            //foreach (int i in ycwflb)
            //{
            //    if (i != 0)
            //        Console.Write(i + ",");
            //}
            //Console.WriteLine();
            Console.WriteLine("共预测" + item.Count + "条数据" + ",其中预测为喜欢" + tp_sum + ",不喜欢" + f_sum);
            //Console.ReadKey();
            Console.WriteLine();
            IService service = new ServiceImpl();
            DataTable yes = service.SelectToDateTable("DBSJ", null, null);//测试集

            int[] szwzlb = new int[yes.Rows.Count];//实际为正的item的id列表
            int[] szwflb = new int[yes.Rows.Count];//实际为负的item的id列表
            int tn_sum = 0;
            int fn_sum = 0;
            foreach (DataRow row in yes.Rows)
            {
                if (int.Parse(row[3].ToString()) >= 3)
                {

                    szwzlb[tn_sum] = int.Parse(row[2].ToString());
                    tn_sum++;
                }
                else
                {

                    szwflb[fn_sum] = int.Parse(row[2].ToString());
                    fn_sum++;
                }
            }
            Console.WriteLine("测试集中共有" + yes.Rows.Count + "个数据，其中" + tn_sum + "个用户喜欢," + fn_sum + "个用户不喜欢");

            double TP = 0;//预测为正，结果为正
            double FP = 0;//预测为正，结果为负
            double FN = 0;//预测为负，结果为正
            double TN = 0;//预测为负，实质为负

            foreach (int i in ycwzlb)
            {
                if (i == 0) continue;
                foreach (int j in szwzlb)
                {
                    if (j == 0) continue;
                    if (i == j)
                    {
                        TP++;
                    }
                }
            }
            foreach (int i in ycwflb)
            {
                if (i == 0) continue;
                foreach (int j in szwflb)
                {
                    if (j == 0) continue;
                    if (i == j)
                    {
                        TN++;
                    }
                }
            }
            FP = tp_sum - TP;
            FN = tn_sum - TP;
            //TN = fn_sum - FP;
            Console.WriteLine("预测为正实质为正的数据TP=" + TP + "预测正实质负的数据：FP=" + FP);
            Console.WriteLine("预测为负实质为负的数据TN=" + TN + "预测负实质正的数据：FN=" + FN);
            Console.WriteLine("精确率=" + TP / (TP + FP) + "，召回率=" + TP / (TP + FN));
            //int n = 0;
            //int[] Rkey = new int[10];
            //foreach (int i in keyArray) {
            //    if (n >= Rkey.Length) break;
            //    if (double.Parse(ht1[i].ToString())>0) {
            //        Console.WriteLine("Item" + i + ",   " + double.Parse(ht1[i].ToString()));
            //        Rkey[n] = i;
            //        n++;
            //    }
            //}
            //
            //DataTable rating = service.SelectToDateTable("YCSJ", null, null);//推测数据

            //int y = 0;
            //foreach (int i in Rkey) {
            //    foreach (DataRow row in yes.Rows) {
            //        int iii = int.Parse((row[2].ToString()));
            //        if (int.Parse((row[2].ToString())) == i) {
            //            y++;
            //        }
            //    }
            //}
            //double d1 = y;
            //double d2 = n;
            //Console.WriteLine("推荐" + n + ",   " + "相关" + y);
            //Console.WriteLine("准确率" +d1/n + ",   " + "召回率" + d1/101);
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
