import React, { Component } from "react";
import ReactDOM from "react-dom";

export class EditUserForm extends Component {
    static displayName = EditUserForm.name;

    constructor(props) {
        super(props);

        this.state = {
            username: "",
            role: 1,
            token: sessionStorage.getItem("token")
        };

        this.onUsernameChange = this.onUsernameChange.bind(this);
        this.onRoleChange = this.onRoleChange.bind(this);
        this.updateUser = this.updateUser.bind(this);
    }

    componentWillUnmount() {
        ReactDOM.unmountComponentAtNode(document.querySelector("#update-user-form"));
    }

    onUsernameChange(e) {
        this.setState({username: e.target.value});
    }

    onRoleChange(e) {
        this.setState({role: parseInt(e.target.value)});
    }

    async updateUser(e) {
        e.preventDefault();

        await fetch(`../api/users/${window.location.pathname.split("/")[3]}`, {
            method: "PUT",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": `Bearer ${this.state.token}`
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .catch(error => alert(error.message));
    }

    render() {
        return  <div id="update-user-form">
            <form onSubmit={this.updateUser}>
                <div className="form-group mb-3">
                        <label htmlFor="new-usernmae" className="control-label">Login:</label>
                        <input id="new-usernmae" className="form-control" type="text" value={this.state.username} onChange={this.onUsernameChange} />
                    </div>
                    <div className="form-group mb-3">
                        <label htmlFor="new-role" className="control-label">Role:</label>
                        <select id="new-role" className="form-control" value={this.state.role} onChange={this.onRoleChange}>
                            <option value="0">Admin</option>
                            <option value="1">User</option>
                        </select>
                    </div>

                    <input className="btn btn-primary" type="submit" value="Submit" />
            </form>
        </div>;
    }
}