export function calculateInterest(amount: number, rate = 0.1) {
    return amount * rate;
}

export function calculateTotalPayable(amount: number, rate = 0.1) {
    return amount + calculateInterest(amount, rate);
}