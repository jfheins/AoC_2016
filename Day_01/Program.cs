using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Day_01
{
    class Program
    {
        private static Vector2 _faceDirection = new Vector2(0, 1);
        private static Matrix3x2 _rotateLeft = Matrix3x2.CreateRotation((float)(Math.PI / 2));
        private static Matrix3x2 _rotateRight = Matrix3x2.CreateRotation((float)(-Math.PI / 2));
        private static List<Vector2> _historicPositions;

        static Vector2 ParseStep(string step, Vector2 pos)
        {
            if (step.Length < 2)
               throw new Exception(step);

            var turn = step[0];

            if (turn == 'L')
            {
                _faceDirection = Vector2.Transform(_faceDirection, _rotateLeft);
            }
            else if (turn == 'R')
            {
                _faceDirection = Vector2.Transform(_faceDirection, _rotateRight);
            }

            var length = int.Parse(step.Substring(1));

            for (int i = 1; i <= length; i++)
            {
                _historicPositions.Add(pos + _faceDirection * i);
            }

            return pos + _faceDirection * length;
        }

        static bool VectorEquals(Vector2 a, Vector2 b)
        {
            bool IsEqual(float x, float y) => Math.Abs(x - y) < 1e-2;
            return IsEqual(a.X, b.X) && IsEqual(a.Y, b.Y);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter input: ");
                var input = "R3, L2, L2, R4, L1, R2, R3, R4, L2, R4, L2, L5, L1, R5, R2, R2, L1, R4, R1, L5, L3, R4, R3, R1, L1, L5, L4, L2, R5, L3, L4, R3, R1, L3, R1, L3, R3, L4, R2, R5, L190, R2, L3, R47, R4, L3, R78, L1, R3, R190, R4, L3, R4, R2, R5, R3, R4, R3, L1, L4, R3, L4, R1, L4, L5, R3, L3, L4, R1, R2, L4, L3, R3, R3, L2, L5, R1, L4, L1, R5, L5, R1, R5, L4, R2, L2, R1, L5, L4, R4, R4, R3, R2, R3, L1, R4, R5, L2, L5, L4, L1, R4, L4, R4, L4, R1, R5, L1, R1, L5, R5, R1, R1, L3, L1, R4, L1, L4, L4, L3, R1, R4, R1, R1, R2, L5, L2, R4, L1, R3, L5, L2, R5, L4, R5, L5, R3, R4, L3, L3, L2, R2, L5, L5, R3, R4, R3, R4, R3, R1";
                //var input = "R8, R4, R4, R8";
                var tokens = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

                var position = new Vector2(0, 0);
                _historicPositions = new List<Vector2>(tokens.Length);
                //_historicPositions.Add(position);

                foreach (var token in tokens)
                {
                    position = ParseStep(token, position);
                }

                foreach (var pos in _historicPositions)
                {
                    int count = _historicPositions.Count(x => VectorEquals(x, pos));
                    var dist = GetManhattanDistance(pos);
                    if (count == 2)
                        Console.WriteLine($"Position {pos} is visited twice. It has a distance of {dist}.");
                }

                Console.WriteLine("Final Location: " + position);
                var manhattanDistance = GetManhattanDistance(position);
                Console.WriteLine("Distance: " + manhattanDistance);
                Console.ReadLine();
                Console.WriteLine();
            }
        }

        private static double GetManhattanDistance(Vector2 position)
        {
            return Math.Round(Math.Abs(position.X) + Math.Abs(position.Y));
        }
    }
}
