package Backend

import java.util.Date


class Block(private val index: Int,
            private val timestamp: Date,
            private val transactions: Array[Transaction],
            private val proof: Int,
            private val previousHash: String) {


  def getIndex: Int = index

  def getTimestamp: Date = timestamp

  def getTransactions: Array[Transaction] = transactions

  def getProof: Int = proof

  def getPreviousHash: String = previousHash

  def getSortedJsonRepresentation: String = {
    s"{'index': '$index', 'proof': '$proof', 'previousHash': '$previousHash', 'timestamp': '$timestamp', 'transactions': '$transactions'"
  }
}
