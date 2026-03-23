import React, { useEffect, useState } from "react";
import { Dependant, FeeRequest } from "../types";
import { fetchDependants } from "../api/dependantApi";
import { fetchFeeRequests } from "../api/feeRequestApi";
import { statusMap, isStatusKey } from "../utils/statusMapper";
import LoanPreview from "../components/LoanPreview";

type LoanPreviewResult = {
    totalPayable: number;
    monthlyRepayment: number;
    payoffDate: string;
};

const ParentDashboard: React.FC = () => {

    const [dependants, setDependants] = useState<Dependant[]>([]);
    const [feeRequests, setFeeRequests] = useState<FeeRequest[]>([]);
    const [loanPreviews, setLoanPreviews] = useState<Record<number, LoanPreviewResult>>({});
    const [loading, setLoading] = useState(true);
    const [selectedDependant, setSelectedDependant] = useState<number>();


    useEffect(() => {

        const loadDashboard = async () => {

            try {

                const deps = await fetchDependants();
                const fees = await fetchFeeRequests();

                const mappedFees: FeeRequest[] = fees.map(f => {

                    const key = isStatusKey(f.status) ? f.status : 0;

                    return {
                        ...f,
                        status: statusMap[key],
                        totalPayable: f.totalPayable ?? f.amount,
                        isPaid: f.isPaid ?? false
                    };

                });

                setDependants(deps);
                setFeeRequests(mappedFees);

                if (deps.length > 0) {
                    setSelectedDependant(deps[0].id);
                }

                /*
                ENTERPRISE FIX
                ONE API CALL instead of N calls
                */

                const previewResponse = await fetch("/Dashboard/GetLoanPreviews");

                const previewData = await previewResponse.json();

                const previews: Record<number, LoanPreviewResult> = {};

                previewData.forEach((p: any) => {

                    previews[p.dependantId] = {
                        totalPayable: p.totalPayable,
                        monthlyRepayment: p.monthlyRepayment,
                        payoffDate: p.payoffDate
                    };

                });

                setLoanPreviews(previews);

            }
            catch (err) {

                console.error("Dashboard load error:", err);

            }
            finally {

                setLoading(false);

            }

        };

        loadDashboard();

    }, []);


    const approvedCount =
        feeRequests.filter(f => f.status === "APPROVED").length;


    const outstandingBalance =
        feeRequests
            .filter(f => f.status === "APPROVED" && !f.isPaid)
            .reduce((sum, f) => sum + (f.totalPayable ?? 0), 0);


    if (loading) return <div>Loading dashboard...</div>;


    return (

        <div className="container mt-4">

            <h2>Parent Dashboard</h2>


            {/* SUMMARY */}

            <div className="row mb-4">

                <div className="col-md-4 card p-3">
                    <h5>Total Dependants</h5>
                    <h3>{dependants.length}</h3>
                </div>

                <div className="col-md-4 card p-3">
                    <h5>Fee Applications</h5>
                    <h3>{feeRequests.length}</h3>
                </div>

                <div className="col-md-4 card p-3">
                    <h5>Approved</h5>
                    <h3>{approvedCount}</h3>
                </div>

            </div>


            {/* DEPENDANTS */}

            <h3>Dependants</h3>

            {dependants.map(d => (

                <div key={d.id} className="card mb-2 p-3">

                    <strong>{d.fullName}</strong><br />

                    Admission #: {d.admissionNumber}<br />

                    Class: {d.classLevel}<br />

                    School ID: {d.schoolId}

                </div>

            ))}


            {/* LOAN CALCULATOR */}

            <h3 className="mt-4">Loan Calculator</h3>

            {selectedDependant && (
                <LoanPreview dependantId={selectedDependant} />
            )}


            {/* LOAN PREVIEW TABLE */}

            <table className="table table-bordered mt-3">

                <thead>

                    <tr>

                        <th>Dependant</th>
                        <th>Total Payable</th>
                        <th>Monthly Repayment</th>
                        <th>Payoff Date</th>

                    </tr>

                </thead>

                <tbody>

                    {dependants.map(d => {

                        const preview = loanPreviews[d.id];

                        return (

                            <tr key={d.id}>

                                <td>{d.fullName}</td>

                                <td>
                                    {preview
                                        ? `Ksh ${preview.totalPayable.toLocaleString()}`
                                        : "Calculating..."
                                    }
                                </td>

                                <td>
                                    {preview
                                        ? `Ksh ${preview.monthlyRepayment.toLocaleString()}`
                                        : "Calculating..."
                                    }
                                </td>

                                <td>
                                    {preview
                                        ? preview.payoffDate
                                        : "Calculating..."
                                    }
                                </td>

                            </tr>

                        );

                    })}

                </tbody>

            </table>


            {/* FEE REQUESTS */}

            <h3 className="mt-4">Fee Requests</h3>

            <table className="table table-bordered">

                <thead>

                    <tr>

                        <th>Dependant</th>
                        <th>Amount</th>
                        <th>Total Payable</th>
                        <th>Status</th>
                        <th>Paid?</th>

                    </tr>

                </thead>

                <tbody>

                    {feeRequests.map(f => (

                        <tr key={f.id}>

                            <td>{f.dependantName}</td>

                            <td>Ksh {f.amount.toLocaleString()}</td>

                            <td>Ksh {f.totalPayable?.toLocaleString()}</td>

                            <td>{f.status}</td>

                            <td>{f.isPaid ? "Yes" : "No"}</td>

                        </tr>

                    ))}

                </tbody>

            </table>


            <h4>
                Outstanding Balance: KES {outstandingBalance.toLocaleString()}
            </h4>

        </div>

    );

};

export default ParentDashboard;