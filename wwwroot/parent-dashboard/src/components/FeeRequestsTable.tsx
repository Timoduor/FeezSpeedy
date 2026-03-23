import React from "react";
import { FeeRequest, LoanStatus } from "../types";

interface Props {
    requests: FeeRequest[];
}

const statusBadge = (status: LoanStatus) => {
    const map: Record<LoanStatus, string> = {
        PENDING: "warning",
        APPROVED: "success",
        REJECTED: "danger",
        DISBURSED: "primary",
    };

    return (
        <span className={`badge bg-${map[status]}`}>
            {status}
        </span>
    );
};

const FeeRequestsTable: React.FC<Props> = ({ requests }) => {
    return (
        <div className="card shadow-sm mt-4">
            <div className="card-body">
                <h5 className="mb-3">Fee Requests</h5>

                <div className="table-responsive">
                    <table className="table table-hover align-middle">
                        <thead className="table-light">
                            <tr>
                                <th>#</th>
                                <th>Dependant</th>
                                <th>School</th>
                                <th>Amount (KES)</th>
                                <th>Status</th>
                                <th>Requested On</th>
                            </tr>
                        </thead>

                        <tbody>
                            {requests.length === 0 ? (
                                <tr>
                                    <td colSpan={6} className="text-center text-muted py-4">
                                        No fee requests found
                                    </td>
                                </tr>
                            ) : (
                                requests.map((req, index) => (
                                    <tr key={req.id}>
                                        <td>{index + 1}</td>
                                        <td>{req.dependantName}</td>
                                        <td>{req.schoolName}</td>
                                        <td>{req.amount.toLocaleString()}</td>
                                        <td>{statusBadge(req.status)}</td>
                                        <td>
                                            {req.requestedOn
                                                ? new Date(req.requestedOn).toLocaleDateString()
                                                : "-"}
                                        </td>
                                    </tr>
                                ))

                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default FeeRequestsTable;