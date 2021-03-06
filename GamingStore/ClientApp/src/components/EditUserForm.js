import React, { Component } from 'react';
import { base64ToArray } from '../modules/converter';
import { formErrorMessage } from '../modules/errorMessageFormer';

export class EditUserForm extends Component {
    static displayName = EditUserForm.name;

    constructor(props) {
        super(props);

        this.state = {
            username: this.props.location.state.username,
            role: this.props.location.state.role,
            profilePicture: JSON.stringify(base64ToArray(this.props.location.state.profilePicture)),
            token: sessionStorage.getItem('token'),
            isObserverAdmin: sessionStorage.getItem('role') === '0'
        };

        this.updateUser = this.updateUser.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    async updateUser(event) {
        event.preventDefault();

        await fetch(`../api/users/${window.location.pathname.split('/')[3]}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify({
                ...this.state,
                role: parseInt(this.state.role)
            })
        })
            .then(response => {
                if (!response.ok) {
                    return formErrorMessage(response);
                }
            })
            .then(() => {
                sessionStorage.setItem('username', this.state.username);
                window.location.href = '/users';
            })
            .catch(error => alert(error));
    }

    handleInputChange = event => {
        const name = event.target.name;
        const value = event.target.value;

        this.setState({
            [name]: value
        });
    }

    handleFileChange = event => {
        const reader = new FileReader();
        const fileByteArray = [];

        reader.readAsArrayBuffer(event.target.files[0]);
        reader.onloadend = evt => {
            if (evt.target.readyState === FileReader.DONE) {
                const arrayBuffer = evt.target.result;
                const array = new Uint8Array(arrayBuffer);

                for (let i = 0; i < array.length; i++) {
                    fileByteArray.push(array[i]);
                }

                this.setState({
                    profilePicture: JSON.stringify(fileByteArray)
                });
            }
        }
    }
    
    render() {
        return <form onSubmit={this.updateUser}>
                <div className="form-group mb-3">
                        <label htmlFor="new-usernmae" className="control-label">Username:</label>
                        <input id="new-usernmae" className="form-control" type="text" name="username" value={this.state.username} onChange={this.handleInputChange} />
                    </div>
                    {
                        this.state.isObserverAdmin &&
                            <div className="form-group mb-3">
                            <label htmlFor="new-role" className="control-label">Role:</label>
                            <select id="new-role" className="form-control" name="role" value={this.state.role} onChange={this.handleInputChange}>
                                <option value="0">Admin</option>
                                <option value="1">User</option>
                            </select>
                        </div>
                    }
                <div className="form-group my-2">
                    <label htmlFor="profile-picture" className="form-label">Choose a picture:</label>
                    <input id="profile-picture" className="form-control" type="file" onChange={this.handleFileChange} />
                </div>

                    <input className="btn btn-primary" type="submit" value="Update" />
            </form>;
    }
}