using System;

public class InsufficientFundsException : Exception
{
    public double current_amount { get; }
    public double balance { get; }

    public InsufficientFundsException(double Current_Amount, double Balance)
        : base($"Insufficient funds! You tried to withdraw {Current_Amount:F2}, but only {Balance:F2} is available.")
    {
        current_amount = Current_Amount;
        balance = Balance;
    }
}

class CampusWallet
{
    static void Main()
    {
        double balance = 0;
        int choice = -1;

        do
        {
            try
            {
                Console.Write("[1] Deposit\n[2] Withdraw\n[3] Show Balance\n[0] Exit\nChoose: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Amount to deposit: ");
                        double deposit = double.Parse(Console.ReadLine());
                        if (deposit <= 0)
                            throw new ArgumentOutOfRangeException(nameof(deposit), "Deposit must be positive.");

                        balance += deposit;
                        Console.WriteLine("Deposit successful.");
                        break;

                    case 2:
                        Console.Write("Amount to withdraw: ");
                        double withdraw = double.Parse(Console.ReadLine());
                        if (withdraw <= 0)
                            throw new ArgumentOutOfRangeException(nameof(withdraw), "Withdrawal must be positive.");

                        if (withdraw > balance)
                            throw new InsufficientFundsException(withdraw, balance);

                        balance -= withdraw;
                        Console.WriteLine("Withdrawal successful.");
                        break;

                    case 3:
                        Console.WriteLine($"Balance: ${balance:F2}");
                        break;

                    case 0:
                        Console.WriteLine("Exiting...");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(choice), "Choice out of range.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input! Please enter numbers only.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("The number is too large!");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            finally
            {
                Console.WriteLine("Ready for the next operation.\n");
            }

        } while (choice != 0);
    }
}
