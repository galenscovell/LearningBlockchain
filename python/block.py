"""

@author GalenS <galen.scovelL@gmail.com>
"""


class Block(dict):
    def __init__(self, index, timestamp, transactions, proof, previous_hash):
        self.index = index
        self.timestamp = timestamp
        self.transactions = transactions
        self.proof = proof
        self.previous_hash = previous_hash

        dict.__init__(
            self,
            index=index,
            timestamp=timestamp,
            transactions=transactions,
            proof=proof,
            previous_hash=previous_hash
        )

