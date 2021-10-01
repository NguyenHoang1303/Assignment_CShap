using System;

namespace Assignment.Util
{
    public static class Confirm
    {
        public static bool HandlerConfirm(string typeConfirm)
        {
            var checkConfirm = false;
            var isConfirm = true;

            while (isConfirm)
            {
                Console.WriteLine($"Do you want to {typeConfirm}:");
                Console.WriteLine("1.Yes");
                Console.WriteLine("2.No");
                Console.WriteLine("Please enter choice:");
                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        checkConfirm = true;
                        isConfirm = false;
                        break;
                    case 2:
                        isConfirm = false;
                        break;
                    default:
                        Console.WriteLine("please re-enter.");
                        break;
                }
            }

            return checkConfirm;
        }
    }
}