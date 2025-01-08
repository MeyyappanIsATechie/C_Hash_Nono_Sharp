using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

class Program
{
    static List<Student> students = new List<Student>();

    public delegate void StudentAddedEventHandler(Student student);
    public static event StudentAddedEventHandler StudentAdded;
    static void Main()
    {
        StudentAdded += NotifyStudentAdded;
        while (true)
        {
            Console.WriteLine("\n1. Add Student\n2. View Students\n3. Delete Student\n4. Filter Students By Age\n5.Save students to file\n6.Load students from file\n7.Exit");
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
                    FilterStudentsByAge();
                    break;
                case "5":
                    SaveStudentsToFile();
                    break;
                case "6":
                    LoadStudentsFromFile();
                    break;
                case "7":
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
        StudentAdded?.Invoke(new Student { ID = id, Name = name, Age = age });
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

    static void FilterStudentsByAge()
    {
        Console.Write("Enter the minimum age to filter: ");
        int age = int.Parse(Console.ReadLine());

        var filteredStudents = students.Where(s => s.Age > age).ToList();

        Console.WriteLine("\nFiltered Students:");
        foreach (var student in filteredStudents)
        {
            Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}");
        }
    }

    static void NotifyStudentAdded(Student student)
    {
        Console.WriteLine($"Event: A new student has been added - ID: {student.ID}, Name: {student.Name}");
    }

    static void SaveStudentsToFile()
    {
        string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("students.json", json);
        Console.WriteLine("Students saved to file.");
    }

    static void LoadStudentsFromFile()
    {
        if (File.Exists("students.json"))
        {
            string json = File.ReadAllText("students.json");
            students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            Console.WriteLine("Students loaded from file.");
        }
        else
        {
            Console.WriteLine("No data file found. Starting with an empty list.");
        }
    }

}

class Student
{
    public int ID { get; set; }

    private string name;
    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
            name = value;
        }
    }

    private int age;
    public int Age
    {
        get => age;
        set
        {
            if (value < 1 || value > 120)
                throw new ArgumentException("Age must be between 1 and 120.");
            age = value;
        }
    }
}
