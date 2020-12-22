using System;
using pulse.core;

namespace pulse.collection
{
    public class Patient
    {
        /*   Properties defenition    */
        private int _id;
        private String _surname;
        private String _name;
        private String _middleName;
        private bool _gender;
        private DateTime _birthdayDate;
        private int _height;
        private int _weight;

        /*  Main constructor    */
        public Patient(int Id) { id = Id; }

        public Patient(int id, string surname, string name, 
            string middleName, bool gender, DateTime birthdayDate,
            int height, int weight) : this(id)
        {
            _surname = surname;
            _name = name;
            _middleName = middleName;
            _gender = gender;
            _birthdayDate = birthdayDate;
            _height = height;
            _weight = weight;
        }


        /*  Getters & Setters   */
        public int id { get => _id; set => _id = value; }
        public string surname { get => _surname; set => _surname = value; }
        public string name { get => _name; set => _name = value; }
        public string middleName { get => _middleName; set => _middleName = value; }
        public bool gender { get => _gender; set => _gender = value; }
        public DateTime birthdayDate { get => _birthdayDate; set => _birthdayDate = value; }
        public int height { get => _height; set => _height = value; }
        public int weight { get => _weight; set => _weight = value; }

        /*  Public functions    */
        public string fullName() {
            return String.Format("{0} {1} {2}", _surname, _name, _middleName);
        }

        public string genderName() {
            return _gender ? "Мужской" : "Женский";
        }

        /*  Database relations   */
        public void create() {
            DBconnection _connection = new DBconnection();
            _connection.insert_patient(this);
        }

        public void update() {
            DBconnection _connection = new DBconnection();
            _connection.update_patient(this);
        }

        public void get() {
            DBconnection _connection = new DBconnection();
            _connection.fill_patient(this);
        }
    }
}
