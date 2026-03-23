import { Disbursement } from "../types";

interface Props {
    disbursements: Disbursement[];
}

const DisbursementsTable = ({ disbursements }: Props) => (
    <table className="table table-bordered mt-4">
        <thead className="table-dark">
            <tr>
                <th>#</th>
                <th>Dependant</th>
                <th>Parent</th>
                <th>School</th>
                <th>Amount</th>
                <th>Paid</th>
                <th>Disbursed At</th>
            </tr>
        </thead>
        <tbody>
            {disbursements.length === 0 ? (
                <tr>
                    <td colSpan={7} className="text-center">
                        No disbursements yet.
                    </td>
                </tr>
            ) : (
                disbursements.map((d, idx) => (
                    <tr key={d.id}>
                        <td>{idx + 1}</td>
                        <td>{d.dependantName}</td>
                        <td>{d.parentName}</td>
                        <td>{d.schoolName}</td>
                        <td>KES {d.amount.toLocaleString()}</td>
                        <td>{d.paid ? "Yes" : "No"}</td>
                        <td>{d.disbursedAt ?? "-"}</td>
                    </tr>
                ))
            )}
        </tbody>
    </table>
);

export default DisbursementsTable;