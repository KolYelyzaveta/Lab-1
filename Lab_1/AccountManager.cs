namespace Manager;
class AccountsManager
{
    public List<BankAccount> Accounts { get; } = new List<BankAccount>(0); 
    public List<int> DelAccounts { get; set; } = new List<int>(0); 
    public List<int> DelDeposits { get; set; } = new List<int>(0); 

    public void AddNewAccount(BankAccount bankAccount) 
    {
        Accounts.Add(bankAccount);
    }
    public void DelAccount(BankAccount bankAccount) 
    {
        Accounts.Remove(bankAccount);
    }
    public BankAccount? FindAccount(int num) 
    {
        foreach (BankAccount i in Accounts)
        {
            if (i.AccountNumber == num)
            {
                return i;
            }
        }
        return null;
    }

    public int GetAccountID()
    {

        if (DelAccounts.Count != 0)
        {
            int temp = DelAccounts[0];
            DelAccounts.RemoveAt(0);
            return temp;
        }
        else if (Accounts.Count == 0)
        {
            return 1;
        }
        else
        {
            int max = 0, value = 0;
            for (int i = 0; i < Accounts.Count; i++)
            {
                value = Accounts[i].AccountNumber;
                if (value > max) max = value;
            }
            return max + 1;
        }
    }

    public void InterestAccrual() 
    {
        DateTime date1 = DateTime.Today;
        DateTime date2 = new DateTime();
        TimeSpan interval = new TimeSpan();
        double interestMoney = 0.0;
        double interestRate = 0.0;

        foreach (BankAccount i in Accounts)
        {
            if (i.Deposits.Count != 0)
            {
                foreach (Deposits d in i.Deposits)
                {
                    date2 = d.LastPayedDate;
                    interval = date1 - date2;
                    while (interval.TotalDays >= 365)
                    {
                        interestRate = (int)d.InterestRate;
                        interestMoney = d.DepAmount * (interestRate / 100);
                        d.DepAmount += interestMoney;
                        //d.LastPayedDate.AddYears(1);
                        Console.Clear();
                        Console.WriteLine($"Відсотки по депозиту № {d.ID} : {interestMoney} грн.");
                        date2 = d.LastPayedDate;
                        interval = date2 - date1;
                    }
                }
            }

        }
    }

    public AccountsManager()
    {
        AddNewAccount(new BankAccount(GetAccountID(), "Кідрук Максим Іванович"));
    }

}

