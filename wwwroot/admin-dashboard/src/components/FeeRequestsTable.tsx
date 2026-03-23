import React from "react";
import { FeeRequest, LoanStatus } from "../types";
import {
    approveRequest,
    rejectRequest,
    disburseRequest,
} from "../services/adminApi";

interface Props {
    feeRequests: FeeRequest[];
    showAdminColumns?: boolean;
}

const FeeRequestsTable: React.FC<Props> = ({
    feeRequests,
    showAdminColumns = false,
}) => {
    const statusBadge = (status: LoanStatus) => {
        const map: Record<LoanStatus, string> = {
            PENDING: "warning",
            APPROVED: "success",
            REJECTED: "danger",
            DISBURSED: "primary",
        };

        return <span className={`badge bg-${map[status]}`}>{status}</span>;
    };

    return (
        <div className="card shadow-sm mt-4">
            <div className="card-body">
                <div className="table-responsive">
                    <table className="table table-hover align-middle">
                        <thead className="table-light">
                            <tr>
                                <th>#</th>
                                <th>Dependant</th>
                                <th>Parent</th>
                                <th>School</th>
                                <th>Amount</th>

                                {showAdminColumns && <th>Total Payable</th>}
                                {showAdminColumns && <th>Status</th>}
                                {showAdminColumns && <th>Actions</th>}

                                <th>Requested On</th>
                            </tr>
                        </thead>

                        <tbody>
                            {feeRequests.length === 0 ? (
                                <tr>
                                    <td
                                        colSpan={showAdminColumns ? 9 : 6}
                                        className="text-center text-muted py-4"
                                    >
                                        No fee requests found
                                    </td>
                                </tr>
                            ) : (
                                feeRequests.map((req, idx) => (
                                    <tr key={req.id}>
                                        <td>{idx + 1}</td>
                                        <td>{req.dependantName}</td>
                                        <td>{req.parentName || "-"}</td>
                                        <td>{req.schoolName}</td>
                                        <td>{req.amount.toLocaleString()}</td>

                                        {showAdminColumns && (
                                            <td>
                                                {req.totalPayable
                                                    ? req.totalPayable.toLocaleString()
                                                    : "-"}
                                            </td>
                                        )}

                                        {showAdminColumns && (
                                            <td>{statusBadge(req.status)}</td>
                                        )}

                                        {showAdminColumns && (
                                            <td>
                                                <div className="d-flex gap-2">
                                                    {req.status === "PENDING" && (
                                                        <>
                                                            <button
                                                                className="btn btn-sm btn-success"
                                                                onClick={() =>
                                                                    approveRequest(req.id)
                                                                }
                                                            >
                                                                Approve
                                                            </button>
                                                            <button
                                                                className="btn btn-sm btn-danger"
                                                                onClick={() =>
                                                                    rejectRequest(req.id)
                                                                }
                                                            >
                                                                Reject
                                                            </button>
                                                        </>
                                                    )}

                                                    {req.status === "APPROVED" && (
                                                        <button
                                                            className="btn btn-sm btn-primary"
                                                            onClick={() =>
                                                                disburseRequest(req.id)
                                                            }
                                                        >
                                                            Disburse
                                                        </button>
                                                    )}
                                                </div>
                                            </td>
                                        )}

                                        <td>
                                            {req.requestedOn
                                                ? new Date(
                                                    req.requestedOn
                                                ).toLocaleDateString()
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