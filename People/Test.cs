using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommend
{
    class Test
    {
        private static MovieLensEntities db = new MovieLensEntities();
        static void Main(string[] args)
        {
            Console.WriteLine("数据初始化");
            var date = new DateTime(1997,9,27);
            var trainingSet = db.ratings.OrderBy(s => s.user_id).Where(s=>s.rated_at < date).ToList();
            var testSet = db.ratings.OrderBy(s => s.user_id).ThenBy(s=>s.movie_id).Where(s => s.rated_at > date).ToList();
            var Users = new List<User>();
            long id = 0;
            bool mark = false;
            User user = null;
            foreach (var training in trainingSet)
            { 
                if (training.user_id != id) {
                    mark = true;
                    id = training.user_id ?? 0;
                }
                if (mark) {
                    user = new User();
                    user.Id = training.user_id.ToString();
                    Users.Add(user);
                    mark = false;
                }
                var item = new Item { Id = training.movie_id.ToString(), Score=training.rating1};
                user.Items.Add(item);
            }
            var recodmmend = new Recommend(Users);
            Console.WriteLine("算法初始化");
            for (var i = 0; i < 20; i++) {
                var K = 5 + i * 5;
                var userId = "1";
                Console.Write("K=" + K);
                recodmmend.Init(K, userId);
                var all = 50;
                var good = 0;
                var bad = 0;
                var result = recodmmend.GetRecommendItems(userId).OrderByDescending(d => d.Score).Skip(0).Take(all).ToList();
                var rating = testSet.Where(t => t.user_id.ToString() == userId).ToList();
                var like = rating.Where(d => d.rating1 >= 3).Count();
                foreach (var r in result)
                {
                    var re = rating.FirstOrDefault(t => t.movie_id.ToString() == r.Id)?.rating1 ?? -1;
                    if (re >= 3) good++;
                    else if (re > 0) { bad++; }
                    //Console.WriteLine(r.Id + "," + r.Score+","+ re);
                }
                Console.WriteLine("  共预测" + all + "   剩余喜欢"+ like + "  预测中" + good + "  预测错"+ bad);
            }
            
            Console.ReadKey();
        }
    }
}
