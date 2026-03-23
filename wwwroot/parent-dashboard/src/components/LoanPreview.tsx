import { useState } from "react";
import axios from "axios";

interface LoanPreviewProps {
    dependantId: number;
}

interface LoanPreviewData {
    TotalPayable: number;
    MonthlyRepayment: number;
    PayoffDate: string;
}

export default function LoanPreview({ dependantId }: LoanPreviewProps) {
    const [amount, setAmount] = useState(0);
    const [duration, setDuration] = useState(6);
    const [interest, setInterest] = useState(12);
    const [preview, setPreview] = useState<LoanPreviewData | null>(null);

    const calculatePreview = async () => {
        try {
            const response = await axios.post<LoanPreviewData>("/Dashboard/CalculateLoanPreview", {
                Amount: amount,
                DurationMonths: duration,
                InterestRate: interest,
                DependantId: dependantId
            });
            setPreview(response.data);
        } catch (err) {
            console.error(err);
            alert("Failed to calculate preview");
        }
    };

    return (
        <div className="loan-preview-form">
            <input
                type="number"
                value={amount}
                onChange={e => setAmount(parseFloat(e.target.value))}
                placeholder="Amount KES"
            />
            <input
                type="number"
                value={duration}
                onChange={e => setDuration(parseInt(e.target.value))}
                placeholder="Duration months"
            />
            <input
                type="number"
                value={interest}
                onChange={e => setInterest(parseFloat(e.target.value))}
                placeholder="Interest %"
            />

            <button onClick={calculatePreview}>Preview Loan</button>

            {preview && (
                <div className="preview-result">
                    <p>Total Payable: Ksh {preview.TotalPayable.toLocaleString()}</p>
                    <p>Monthly Repayment: Ksh {preview.MonthlyRepayment.toLocaleString()}</p>
                    <p>Payoff Date: {preview.PayoffDate}</p>
                </div>
            )}
        </div>
    );
}