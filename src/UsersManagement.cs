using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace TeacherARMBackend {

    class Session {
        public string Token;

        public User User;

        public DateTime LastTime;
    }

    class SessionManager {
        public static TimeSpan CLOSE_TIME = TimeSpan.FromMinutes(10);
        private DataBaseAccessor _accessor = new DataBaseAccessor();
        private Dictionary<string, Session> Sessions = new Dictionary<string, Session>();
        //Возвращает null если не прошел автризацию
        private Thread _sesCloseThread;

        public void EternalClosing() {
            while (true) {
                CloseAllExpiredSessions();
                Thread.Sleep(5000);
            }
        }


        public SessionManager() {
            _sesCloseThread = new Thread(new ThreadStart(this.EternalClosing));
            _sesCloseThread.Start();
        }
        public Session Authorize(string login, string pass) {      
            var user = _accessor.ExecuteWithResult($"SELECT id, name, surname, patronymic, login, password, is_admin FROM arm_user WHERE login='{login}' and password='{pass}'").Select(x => new User {
                        id = (uint) x[0],
                        name = (string) x[1],
                        surname = (string) x[2],
                        patronymic = (string) x[3],
                        login = (string) x[4],
                        password = (string) x[5],
                        is_admin = (bool) x[6]                                                
                    });

            if (user.Any()) {
                Session session = new Session() {
                    Token = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""),                    
                    LastTime = DateTime.Now,
                    User = user.First()  
                };
                Sessions.Add(session.Token, session);
                return session;
            } else 
                return null;
        }

        public Session CheckToken(string token) {
            if (Sessions.TryGetValue(token, out Session value)) {
                value.LastTime = DateTime.Now;
                return value;
            } else {
                return null;
            }
        }

        private void CloseAllExpiredSessions() {
            List<String> toRemove = new List<String>();
            foreach (var pair in Sessions) {                
                if ( ( DateTime.Now - pair.Value.LastTime) > CLOSE_TIME) {
                    Console.WriteLine("Remove: " + pair.Key);
                    toRemove.Add(pair.Key);
                }
            }
            toRemove.ForEach(x => Sessions.Remove(x));
        }

    }


}