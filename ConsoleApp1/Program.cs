using System;
using System.Collections.Generic;

class Program
{
    static List<Student> students = new List<Student>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Add Student\n2. View Students\n3. Delete Student\n4. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    ViewStudents();
                    break;
                case "3":
                    DeleteStudent();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void AddStudent()
    {
        Console.Write("Enter Student Name: ");
        string name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        Console.Write("Enter Student Age: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0)
        {
            Console.WriteLine("Invalid age. Please enter a positive integer.");
            return;
        }
        int id = students.Count + 1;

        students.Add(new Student { ID = id, Name = name, Age = age });
        Console.WriteLine("Student added successfully.");
    }

    static void ViewStudents()
    {
        try
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No students available.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while viewing the students: {ex.Message}");
        }
    }

    static void DeleteStudent()
    {
        try
        {
            Console.Write("Enter Student ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("Invalid ID. Please enter a valid positive integer.");
                return;
            }

            var student = students.Find(s => s.ID == id);
            if (student != null)
            {
                students.Remove(student);
                Console.WriteLine("Student removed.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the student: {ex.Message}");
        }
    }
}

class Student
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
