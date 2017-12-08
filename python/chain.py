"""
The core blockchain.

@author GalenS <galen.scovelL@gmail.com>
"""

from time import time

from block import Block
from transaction import Transaction

from urllib.parse import urlparse


class Chain(object):
    def __init__(self):
        self.chain = []
        self.current_transactions = []
        self.nodes = set()

        # Create the genesis block with no predecessors
        self.new_block(proof=100, previous_hash=1)

    def register_node(self, address):
        parsed_url = urlparse(address)
        self.nodes.add(parsed_url.path)

    def new_block(self, proof, previous_hash=None):
        """
        Creates a new Block and adds it to the chain
        """
        block = Block(
            index=len(self.chain) + 1,
            timestamp=time(),
            transactions=self.current_transactions,
            proof=proof,
            previous_hash=previous_hash
        )

        self.current_transactions.clear()
        self.chain.append(block)

        return block

    def new_transaction(self, sender, recipient, amount):
        """
        Adds a new transaction to the list of transactions
        """
        self.current_transactions.append(
            Transaction(sender, recipient, amount)
        )

        return self.last_block.index + 1

    @property
    def last_block(self):
        """
        Returns the last Block in the chain
        """
        return self.chain[-1]
