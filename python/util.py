"""

@author GalenS <galen.scovelL@gmail.com>
"""

import hashlib
import json
import requests


def valid_chain(chain):
    """
    Determines if a chain is valid
    """
    last_block = chain[0]
    current_idx = 1

    while current_idx < len(chain):
        block = chain[current_idx]
        if block.previous_hash != hash(last_block):
            return False

        last_block = block
        current_idx += 1

    return True


def resolve_conflicts(our_chain):
    """
    Consensus algorithm
    We're keeping it simple -- the longest valid chain in the network is authoritative
    """
    neighbours = our_chain.nodes
    new_chain = None
    max_length = len(our_chain.chain)

    for node in neighbours:
        response = requests.get(f'http://{node}/chain')

        if response.status_code == 200:
            length = response.json()['length']
            chain = response.json()['chain']

            if length > max_length and valid_chain(chain):
                max_length = length
                new_chain = chain

    if new_chain:
        our_chain.chain = new_chain
        return True

    return False


def proof_of_work(last_proof):
    """
    Simple Proof of Work Algorithm:
    - Find a number p' such that hash(pp') contains leading 4 zeroes, where p is the previous p'
    - p is the previous proof, and p' is the new proof
    """
    proof = 0
    while valid_proof(last_proof, proof) is False:
        proof += 1

    return proof


def valid_proof(last_proof, proof):
    """
    Validates the proof: Does hash contain 4 leading zeroes?
    """
    guess = f'{last_proof}{proof}'.encode()
    guess_hash = hashlib.sha256(guess).hexdigest()

    return guess_hash[:4] == '0000'


def hash(block):
    """
    Creates a SHA-256 hash of a Block with ordered Dict for consistent hashes
    """
    block_string = json.dumps(block, sort_keys=True).encode()

    return hashlib.sha256(block_string).hexdigest()

