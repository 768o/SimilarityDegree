using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommend
{
    class Recommend
    {
        public List<User> Users { get; set; }
        public List<string> _ConmonItem { get; set; }

        public Recommend(List<User> Users) {
            this.Users = Users;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="K">取最相似的前K用户</param>
        /// <param name="id">如果不传此id，将初始化所有用户，很耗时间</param>
        public void Init(int K = 10,string id = "-1")
        {
            foreach (var curUser in Users)
            {
                if (!id.Equals("-1"))
                {
                    if (!curUser.Id.Equals(id)) continue;
                }
                curUser.RecommendItems = new List<Item>();
                curUser.UserDegrees = new List<Item>();
                foreach (var allUser in Users)
                {
                    if (curUser.Id.Equals(allUser.Id)) continue;
                    else {
                        var EM = 0.0;
                        foreach (var allUserItem in allUser.Items) {
                            var Exist = false;
                            foreach (var curUserItem in curUser.Items) {
                                if (curUserItem.Id.Equals(allUserItem.Id)) {
                                    EM += Math.Pow(allUserItem.Score - curUserItem.Score, 2);//欧几里德距离
                                    Exist = true;
                                    continue;
                                }
                            }
                            if (!Exist)
                            {
                                if (curUser.RecommendItems.FirstOrDefault(d => d.Id == allUserItem.Id) == null)
                                {
                                    curUser.RecommendItems.Add(new Item { Id = allUserItem.Id });
                                }
                            }
                        }
                        var UserDegree = new Item {
                            Id = allUser.Id,
                        };
                        if (EM == 0.0)
                        {
                            UserDegree.Score = -1;
                        }
                        else {
                            UserDegree.Score = 1 / (1 + Math.Sqrt(EM));
                        }
                        curUser.UserDegrees.Add(UserDegree);
                    }
                }
                break;
            }

            foreach (var user in Users) {
                if (!id.Equals("-1"))
                {
                    if (!user.Id.Equals(id)) continue;
                }
                var degreeItemsTemp = user.UserDegrees.OrderByDescending(d => d.Score).ToList();
                var degreeItems = degreeItemsTemp.Skip(0).Take(K).ToList();
                foreach (var recommendItem in user.RecommendItems)
                {
                    var recommendScore = 0.0;
                    foreach (var degreeItem in degreeItems) {
                        var degreeUser = Users.FirstOrDefault(u => u.Id == degreeItem.Id);
                        var degreeUserItem = degreeUser.Items.FirstOrDefault(d=>d.Id==recommendItem.Id);
                        if (degreeUserItem != null) {
                            recommendScore += degreeUserItem.Score * degreeItem.Score;
                        }
                    }
                    recommendItem.Score = recommendScore;
                }
            }
        }

        public List<Item> GetRecommendItems(string UserId)
        {
            return Users.FirstOrDefault(u=>u.Id == UserId).RecommendItems;
        }

        public List<Item> GetUserDegrees(string UserId)
        {
            return Users.FirstOrDefault(u => u.Id == UserId).UserDegrees;
        }
    }
}
