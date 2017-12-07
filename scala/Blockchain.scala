package Backend

import java.util.Date

import scala.collection.mutable.ArrayBuffer


class Blockchain {
  val chain: ArrayBuffer[Block] = new ArrayBuffer[Block]()
  val currentTransactions: ArrayBuffer[Transaction] = new ArrayBuffer[Transaction]()


  def newBlock(proof: Int, previousHash: String): Block = {
    val block: Block = new Block(
      index = chain.length + 1,
      timestamp = new Date(),
      transactions = currentTransactions.toArray,
      proof = proof,
      previousHash = previousHash
    )

    currentTransactions.clear()
    chain += block

    block
  }

  def newTransaction(sender: String, recipient: String, amount: Double): Unit = {
    val transaction: Transaction = new Transaction(sender, recipient, amount)
    currentTransactions += transaction

    getLastBlock.getIndex + 1
  }

  def proofOfWork(lastProof: Int): Int = {
    var proof: Int = 1
    while (!validateProof(lastProof, proof)) {
      proof += 1
    }

    proof
  }

  def hashBlock(block: Block): String = {
    sha256Hash(block.getSortedJsonRepresentation)
  }

  private def validateProof(lastProof: Int, proof: Int): Boolean = {
    val guess = s"$lastProof$proof"
    val guessHash = sha256Hash(guess)

    guessHash.startsWith("0000")
  }

  private def sha256Hash(text: String): String = {
    String.format("%064x", new java.math.BigInteger(
        1, java.security.MessageDigest.getInstance("SHA-256").digest(text.getBytes("UTF-8"))
      )
    )
  }

  private def getLastBlock: Block = {
    chain.last
  }
}
