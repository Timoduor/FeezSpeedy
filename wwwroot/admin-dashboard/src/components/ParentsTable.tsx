import React from "react";
import { Parent } from "../types";

interface Props { parents: Parent[]; }

const ParentsTable: React.FC<Props> = ({ parents }) => (
    <table className="table table-bordered mt-4">
        <thead className="table-dark">
            <tr>
                <th>#</th>
                <th>Full Name</th>
                <th>Email</th>
            </tr>
        </thead>
        <tbody>
            {parents.length === 0 ? (
                <tr>
                    <td colSpan={3} className="text-center">No parents yet.</td>
                </tr>
            ) : (
                parents.map((p, idx) => (
                    <tr key={p.id}>
                        <td>{idx + 1}</td>
                        <td>{p.fullName}</td>
                        <td>{p.email || "-"}</td>
                    </tr>
                ))
            )}
        </tbody>
    </table>
);

export default ParentsTable;