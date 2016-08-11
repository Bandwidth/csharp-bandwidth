using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net.Api
{
  /// <summary>
  /// Access to Account Api
  /// </summary>
  public interface IAccount
  {
    /// <summary>
    /// Get information about account
    /// </summary>
    /// <param name="cancellationToken">Optional token to cancel async operation</param>
    /// <returns>Task with <see cref="Account"/> Account instance</returns>
    /// <example>
    /// <code>
    /// var account = await client.Account.Get();
    /// </code>
    /// </example>
    Task<Account> GetAsync(CancellationToken? cancellationToken = null);

    /// <summary>
    /// Get a list of the transactions made to account
    /// </summary>
    /// <param name="query">Optional query parameters</param>
    /// <param name="cancellationToken">>Optional token to cancel async operation</param>
    /// <returns>Collection with <see cref="AccountTransaction"/> instances</returns>
    /// <example>
    /// <code>
    /// var transactions = client.Account.GetTransactions();
    /// </code>
    /// </example>
    IEnumerable<AccountTransaction> GetTransactions(AccountTransactionQuery query = null,
      CancellationToken? cancellationToken = null);
  }

  internal class AccountApi: ApiBase, IAccount
  {
    public Task<Account> GetAsync(CancellationToken? cancellationToken = null)
    {
      return Client.MakeJsonRequestAsync<Account>(HttpMethod.Get, $"/users/{Client.UserId}/account", cancellationToken);
    }

    public IEnumerable<AccountTransaction> GetTransactions(AccountTransactionQuery query = null, CancellationToken? cancellationToken = null)
    {
      return new LazyEnumerable<AccountTransaction>(Client, () => Client.MakeJsonRequestAsync(HttpMethod.Get, $"/users/{Client.UserId}/account/transactions", cancellationToken, query));
    }
  }

  /// <summary>
  /// Account information
  /// </summary>
  public class Account
  {
    /// <summary>
    /// Account balance in dollars, as a string; the currency symbol is not included.
    /// </summary>
    public string Balance { get; set; }

    /// <summary>
    /// The type of account configured foruser
    /// </summary>
    public AccountType AccountType { get; set; }
  }

  /// <summary>
  /// Query to get account transactions
  /// </summary>
  public class AccountTransactionQuery
  {
    /// <summary>
    /// Limit the number of transactions that will be returned.
    /// </summary>
    public int? MaxItems { get; set; }

    /// <summary>
    /// Return only transactions that are newer than the parameter
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Return only transactions that are older than the parameter
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Return only transactions that are this type.
    /// </summary>
    public AccountTransactionType? Type { get; set; }

    /// <summary>
    /// Return only transactions that are from the specified number
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Used for pagination to indicate the size of each page requested for querying a list of transactions. If no value is specified the default value is 25 (maximum value 1000).
    /// </summary>
    public int? Size { get; set; }
  }

  /// <summary>
  /// Account transaction
  /// </summary>
  public class AccountTransaction
  {
    /// <summary>
    /// The unique identifier for the transaction.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The time the transaction was processed.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// The transaction amount in dollars, as a string; the currency symbol is not included.
    /// </summary>
    public string Amount { get; set; }

    /// <summary>
    /// The type of transaction.
    /// </summary>
    public AccountTransactionType Type { get; set; }

    /// <summary>
    /// The number of product units the transaction charged or credited.
    /// </summary>
    public string Units { get; set; }

    /// <summary>
    /// The phone number the transaction was related to (not all transactions are related to a phone number).
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// The product the transaction was related to (not all transactions are related to a product).
    /// </summary>
    public ProductType ProductType { get; set; }
  }

  /// <summary>
  /// Account types
  /// </summary>
  public enum AccountType
  {
    /// <summary>
    /// The type of account where you increase your available balance with credit card payments.
    /// </summary>
    PrePay
  }

  /// <summary>
  /// Account transaction types
  /// </summary>
  public enum AccountTransactionType
  {
    /// <summary>
    /// A charge for the use of a service or resource (for example, phone calls, SMS messages, phone numbers).
    /// </summary>
    Charge,

    /// <summary>
    /// A payment you made to increase your account balance.
    /// </summary>
    Payment,

    /// <summary>
    /// An increase to your account balance that you did not pay for (for example, an initial account credit or promotion).
    /// </summary>
    Credit,

    /// <summary>
    /// An automated payment made to keep your account balance above the minimum balance you configured.
    /// </summary>
    AutoRecharge
  }

  /// <summary>
  /// Product types
  /// </summary>
  public enum ProductType
  {
    /// <summary>
    /// The monthly charge for a local phone number.
    /// </summary>
    LocalNumberPerMonth,

    /// <summary>
    /// The monthly charge for a toll-free phone number.
    /// </summary>
    TollFreeNumberPerMonth,

    /// <summary>
    /// A SMS message that came in to one of your numbers. 
    /// </summary>
    SmsIn,

    /// <summary>
    /// A SMS message that was sent outbound one of your numbers.
    /// </summary>
    SmsOut,

    /// <summary>
    /// A MMS message that was sent to one of your numbers.
    /// </summary>
    MmsIn,

    /// <summary>
    /// A MMS message that was sent outbound one of your numbers.
    /// </summary>
    MmsOut,

    /// <summary>
    /// An inbound phone call to one of your numbers.
    /// </summary>
    CallIn,

    /// <summary>
    /// An outbound phone call that was created by your app.
    /// </summary>
    CallOut,

    /// <summary>
    /// A phone call that came inbound via SIP to one of your registered endpoints.
    /// </summary>
    SipCallIn,

    /// <summary>
    /// A phone call made outbound to a SIP address.
    /// </summary>
    SipCallOut,

    /// <summary>
    /// A transcription of a recorded call.
    /// </summary>
    Transcription,

    /// <summary>
    /// A CNAM lookup request for a phone number.
    /// </summary>
    CnamSearch
  }
}
