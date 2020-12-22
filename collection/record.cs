using System;
using pulse.core;

namespace pulse.collection
{
    public class Record
    {
        /* Varible definition */
        private string _id;
        private DateTime _time;
        private float _duration;
        private String _comments;
        private Patient _patient;

        /* Main constructors */
        public Record(float duration, Patient patient)
        {
            _id = Guid.NewGuid().ToString();
            _time = DateTime.Now;
            _duration = duration;
            _patient = patient;
        }
        public Record(float duration, int patient)
        {
            _id = Guid.NewGuid().ToString();
            _time = DateTime.Now;
            _duration = duration;
            _patient = new Patient(patient);
        }
        public Record(string Id, DateTime Time, float Duration, string Comments, int Patient)
        {
            _id = Id;
            _time = Time;
            _duration = Duration;
            _comments = Comments;
            _patient = new Patient(Patient);
        }
        public Record(string Id, DateTime Time, float Duration, string Comments, Patient Patient)
        {
            _id = Id;
            _time = Time;
            _duration = Duration;
            _comments = Comments;
            _patient = Patient;
        }

        /* Getters & Setters */
        public string id { get => _id; set => _id = value; }
        public DateTime time { get => _time; set => _time = value; }
        public float duration { get => _duration; set => _duration = value; }
        public string comments { get => _comments; set => _comments = value; }
        public Patient patient { get => _patient; set => _patient = value; }


        /*  Database relations   */
        public void create()
        {
            DBconnection _connection = new DBconnection();
            _connection.insert_record(this);
        }
        public void update()
        {
            DBconnection _connection = new DBconnection();
            _connection.update_record(this);
        }

    }
}
