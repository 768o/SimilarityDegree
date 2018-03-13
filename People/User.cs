using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommend
{
    class User
    {
        public int id { set; get; }
        public List<Grade> ListGrade { set; get; }

        public Hashtable GetFriendlyUser(List<User> ListUser)
        {
            Hashtable ht = null;
            if (ListUser != null)
            {
                ht = new Hashtable();
                foreach (User user in ListUser)//遍历所有用户
                {
                    if (id == user.id) continue;//跳过自己
                    else
                    {
                        double Euclidean_Metric = GetSimilarityDegree(user.ListGrade);//获得目标用户与其他用户之间的相似度
                        ht.Add(user.id, Euclidean_Metric);//把某用户id与其对应的相似度加入Hashtable
                    }
                }
            }
            return ht;
        }

        private double GetSimilarityDegree(List<Grade> ListGrade) {
            double EM = 0;//欧几里得距离中各项的E(x-y)的平方的和
            if (this.ListGrade != null)  
                foreach (Grade user_grade in this.ListGrade)//遍历自己的所有评分
                    foreach (Grade other_user_grade in ListGrade)//遍历其他用户的所有评分
                        if (user_grade.Item == other_user_grade.Item)//如果两用户对同一个Item评过分
                            EM += Math.Pow((user_grade.Score - other_user_grade.Score), 2);
            return 1 / (1 + Math.Sqrt(EM));//两用户相似度
        }
    }
}
