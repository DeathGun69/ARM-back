using System;
using System.Threading;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace TeacherARMBackend {

    enum Role {
        USER
    }
    class Session {
        public string Token;
        public Role Role;

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
            Console.WriteLine(login + " " + pass);            
            if (_accessor.ExecuteWithResult($"SELECT id FROM arm_user WHERE login='{login}' and password='{pass}'").Any()) {
                Session session = new Session() {
                    Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                    LastTime = DateTime.Now                    
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