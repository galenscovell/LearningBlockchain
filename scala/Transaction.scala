package Backend


class Transaction(private val sender: String,
                  private val recipient: String,
                  private val amount: Double) {

  def getSender: String = sender

  def getRecipient: String = recipient

  def getAmount: Double = amount
}
