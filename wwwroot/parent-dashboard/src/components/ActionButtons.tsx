import React from "react";
import { useNavigate } from "react-router-dom";

const ActionButtons: React.FC = () => {

    const navigate = useNavigate();

    return (
        <div className="d-flex gap-2">

            <button
                onClick={() => navigate("/parent/create-dependant")}
                className="btn btn-primary"
            >
                + Add Dependant
            </button>

            <button
                onClick={() => navigate("/parent/create-fee-request")}
                className="btn btn-outline-secondary"
            >
                New Fee Request
            </button>

        </div>
    );
};

export default ActionButtons;