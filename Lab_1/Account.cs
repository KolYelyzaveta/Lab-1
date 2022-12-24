using Manager;
using System.Text;

class BankAccount
{
    public string FullName { get; set; }
    public int AccountNumber { get; set; }
    public double Amount { get; set; } = 0;

    public List<Deposits> Deposits { get; set; } = new List<Deposits>(0); 

    public BankAccount(int num, string fullName)
    {
        FullName = fullName;
        AccountNumber = num; 
    }
   
    public int GetDepositID(AccountsManager main)
    {

        if (main.DelDeposits.Count != 0)
        {
            int temp = main.DelDeposits[0];
            main.DelDeposits.RemoveAt(0);
            return temp;
        }
        else if (Deposits.Count == 0)
        {
            return 1;
        }
        else
        {
            int max = 0, value = 0;
            for (int i = 0; i < Deposits.Count; i++)
            {
                value = Deposits[i].ID;
                if (value > max) max = value;
            }
            return max + 1;
        }
    }

    public void CreateDeposit(int id, double amount, int interestRate) 
    {
        Deposits.Add(new Deposits(id, amount, (InterestRate)interestRate));
    }

    public bool DeleteDeposit(AccountsManager mainAcc, Deposits deposit) 
    {
        Amount += deposit.DepAmount; 

        mainAcc.DelDeposits.Add(deposit.ID); 

        return Deposits.Remove(deposit); 
    }

    public Deposits? FindDeposits(int num) 
    {
        foreach (Deposits i in Deposits)
        {
            if (i.ID == num)
            {
                return i;
            }
        }
        return null;
    }

    public double AmountOnDeposits()
    {
        double sum = 0.0;
        foreach (Deposits i in Deposits)
        {
            sum += i.DepAmount;
        }
        return sum;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder($"ПІБ: {FullName} \n Номер рахунку: {AccountNumber} \n Сума: {Amount}");
        if (Deposits.Count == 0)
        {
            sb.Append(", депозитів немає.");
        }
        else
        {
            sb.Append($", номера депозитів: ");
            for (int i = 0; i < Deposits.Count; i++)
            {
                sb.Append(Convert.ToString(Deposits[i].ID));
                if (i != Deposits.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(".");
        }
        return sb.ToString();
    }
}
