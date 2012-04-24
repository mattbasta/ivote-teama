<%@ Page Title="Committee Elections" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="committee_election" CodeFile="committee_election.aspx.cs" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Committee Election</li>
    </ul>
    
    <div class="page-header">
        <h1><asp:Literal ID="CommitteeNameLiteral" runat="server" /> Election</h1>
    </div>
    
    <asp:Panel runat="server" ID="JulioButtonPanel" CssClass="well">
        <asp:LinkButton ID="CancelElection" Visible="true" Text="Cancel Election"
                OnClick="CancelElection_Click" runat="server" CssClass="btn btn-danger pull-right" />
        <p><big><strong>Current Phase:</strong> <asp:Literal ID="PhaseLiteral" Text="Inactive" runat="server" /></big></p>
        <p>
            <asp:Literal ID="DaysRemaining" Text="No election is currently in progress." runat="server" />
            <a href="#" onclick="$('#MainContent_phasedelta').toggle();return false;" runat="server" id="phasedeltaedit" Visible="false">(edit)</a>
        </p>
        <p id="phasedelta" style="display:none" runat="server" Visible="false">
            <asp:TextBox ID="DeltaText" runat="server" TextMode="SingleLine" MaxLength="2"  />
            <asp:NumericUpDownExtender ID="Delta" runat="server" TargetControlID="DeltaText" Width="55" Maximum="14" Minimum="-14" />
            days remain in this phase.
            <asp:Button ID="DeltaSubmit" Text="Set Days Remaining" runat="server" CssClass="btn btn-small" OnClick="DeltaSubmit_Click" />
        </p>
        <asp:Panel runat="server" ID="JulioButtonHider" CssClass="form form-inline juliobuttonbox" Visible="false">
            <asp:Button runat="server" ID="JulioButton" Text="Switch to Next Phase"
                    CssClass="btn btn-primary btn-small" OnClick="JulioButton_Clicked" />
            <asp:Literal runat="server" Text="or switch to " ID="JulioButtonSpacerText" Visible="true" />
            <asp:DropDownList runat="server" ID="JulioButtonPhase">
                <asp:ListItem Value="WTSPhase">WTS</asp:ListItem>
                <asp:ListItem Value="NominationPhase">Primary Election</asp:ListItem>
                <asp:ListItem Value="VotePhase">Voting</asp:ListItem>
                <asp:ListItem Value="CertificationPhase">Certification</asp:ListItem>
                <asp:ListItem Value="ConflictPhase">Conflict Resolution</asp:ListItem>
            </asp:DropDownList>
            <asp:Button runat="server" ID="JulioButtonCustom" Text="Switch"
                    CssClass="btn" OnClick="JulioButtonCustom_Clicked" />
            <script type="text/javascript">
            <!--
            $("#MainContent_JulioButton").click(function() {
                var t = $(this);
                if(t.hasClass("disabled"))
                    t.attr("href", "#");
                t.attr("disabled", "disabled");
                t.addClass("disabled");
            });
            -->
            </script>
        </asp:Panel>
    </asp:Panel>
    
    <div class="well">
        <big><b><asp:Literal runat="server" id="VacancyCount" /></b> position(s) are being voted on in this election.</big>
    </div>
    
    <asp:ToolkitScriptManager ID="AJAXManager" runat="Server" />

    <asp:Panel ID="AdminTabs" runat="server" Visible="false">
        <div class="tabbable">
            <ul class="nav nav-tabs">
                <li runat="server" ID="wts_tab" Visible="false">
                    <asp:LinkButton Text="Willingness to Serve" runat="server"
                            OnClick="Tab_Clicked" CommandName="WTSPhase" />
                </li>
                <li runat="server" ID="nominations_tab" Visible="false">
                    <asp:LinkButton Text="Primary Nominations" runat="server"
                            OnClick="Tab_Clicked" CommandName="NominationPhase" />
                </li>
                <li runat="server" ID="votes_tab" Visible="false">
                    <asp:LinkButton Text="Votes" runat="server"
                            OnClick="Tab_Clicked" CommandName="VotePhase" />
                </li>
                <li runat="server" ID="certifications_tab" Visible="false">
                    <asp:LinkButton Text="NEC Certifications" runat="server"
                            OnClick="Tab_Clicked" CommandName="CertificationPhase"
                            ID="certifications_tab_link" />
                </li>
                <li runat="server" ID="conflicts_tab" Visible="false">
                    <asp:LinkButton Text="Election Conflicts" runat="server"
                            OnClick="Tab_Clicked" CommandName="ConflictPhase" />
                </li>
                <li runat="server" ID="closed_tab" Visible="false">
                    <asp:LinkButton Text="Final Results" runat="server"
                            OnClick="Tab_Clicked" CommandName="ClosedPhase" />
                </li>
            </ul>
        </div>
    </asp:Panel>

    <asp:Panel ID="AdminWTSPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Revoke Willingness to Serve</legend>
            <asp:Panel ID="wtsAdminConfirm" runat="server" Visible="false" CssClass="alert alert-info">
                <strong>WTS Revoked</strong>
                User's willingness to serve has been revoked.
            </asp:Panel>
            <asp:Panel ID="wtsAdminList" runat="server">
                <asp:Table ID="wtsAdminTable" runat="server" CssClass="table table-bordered">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Department</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Actions</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </asp:Panel>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="AdminNominationsPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Review Primary Election Nominations</legend>
            <asp:Table ID="AdminNominationsTable" CssClass="table table-bordered" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Will Be Candidate?</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="AdminVotingPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Review Vote Tally</legend>
            <asp:Table ID="AdminVotingTable" CssClass="table table-bordered" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="NECCertificationPanel" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>Review Vote Tally</legend>
            <asp:Panel ID="NECCertificationComplete" runat="server" Visible="false" CssClass="alert alert-success">
                <strong>Certified</strong>
                Thank you for certifying the election results!
            </asp:Panel>
            <asp:Table ID="NECVotingTable" CssClass="table table-bordered" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
                </asp:TableHeaderRow>            
            </asp:Table>
            <div id="NECCertifyAgreement" runat="server">
                <p>As a member of the NEC, I certify that these election results are valid and have been properly collected.</p>
                <div class="control-group">
                    <label class="control-label">Confirm</label>
                    <div class="controls">
                        <label class="checkbox">
                            <asp:CheckBox ID="CertifyCheckBox" Checked="false" runat="server" />
                            I hereby certify this election's results.
                        </label>
                        <asp:Label ID="CertifyWarning" Text="Please tick the certification check box before submitting your certification!"
                            CssClass="help-block error" runat="server" Visible="false" />
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="CertifyButton" Text="Submit Certification"
                        runat="server" OnClick="Certify_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </fieldset>
    </asp:Panel>


    <asp:Panel ID="AdminCertificationPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Review NEC Certification Activity</legend>
            <p><asp:Label ID="AdminCertCount" Visible="true" runat="server" /></p>
            <div class="progress progress-success">
                <div class="bar" ID="necprogressbar" runat="server"></div>
            </div>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="AdminConflictPanel" runat="server" Visible="false">
        <fieldset id="conflicts_to_resolve" runat="server">
            <legend>Conflict Resolution</legend>
            <p><asp:Label ID="AdminNoConflicts" Visible="false" Text="There are no conflicts to resolve." runat="server" /></p>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="AdminClosedPanel" runat="server" Visible="false" CssClass="alert">
        <strong>Election Closed</strong>
        This election is currently closed. You can review the vote counts on the <b>Votes</b> tab.
        <asp:Button ID="GeneratePDFButton" text="View Printable Results Form" CssClass="btn btn-small" runat="server" OnClick="GeneratePDFButton_Click" /> 
    </asp:Panel>
    
    
    <asp:Panel ID="FacultyClosed" runat="server" Visible="false" CssClass="alert">
        <strong>Election Closed</strong>
        This election is currently closed. Thank you for participating.
    </asp:Panel>

    <asp:Panel ID="FacultyWTS" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>Willingness to Serve</legend>
            <asp:Panel ID="wtsPanelServing" runat="server" Visible="false" CssClass="alert alert-info">
                <strong>Already Serve</strong>
                You already serve on this contractual committee.
            </asp:Panel>
            <asp:Panel ID="wtsPanelExisting" runat="server" Visible="false" CssClass="alert alert-info">
                <strong>Willingness to Serve Submitted</strong>
                You have already submitted a willing to serve form for this election.
            </asp:Panel>
            <asp:Panel ID="wtsPanelDone" runat="server" Visible="false" CssClass="alert alert-success">
                <strong>Willingness to Serve Submitted</strong>
                Your willingness to serve has been successfully registered.
            </asp:Panel>
            <asp:Panel ID="wtsPanelLength" runat="server" Visible="false" CssClass="alert alert-warning">
                <strong>Not Submitted</strong>
                Your statement is too long. You are limited to 1000 characters (about two paragraphs).
            </asp:Panel>
            <p><asp:Literal ID="CommitteeDescription" runat="server" /></p>
            <ol>
                <li>No faculty member may serve on more than one contract committee (Promotion, Tenure, and Sabbatical Leave)</li>
                <li>No faculty member shall be able to serve on a university-wide committee when he/she or a member of his or her immediate family or a person residing in his or her household is an applicant to that committee.</li>
                <li>Only one faculty member from a department may serve on any contract committee at one time.</li>
                <li>The APSCUF/KU President, Meet and Discuss Spokesperson, and Grievance Chair may not serve on any contract committee.</li>
                <li runat="server" id="must_be_tenured">A faculty member must be tenured prior to his or her nomination to serve on this committee.</li>
            </ol>
            <p>
                If you wish to serve on the <asp:Literal ID="CommitteeNameLiteral2" runat="server" />, please state your
                views on the committee's function and reasons for wanting to serve. This information will be distributed with
                the ballot.
            </p>
            <asp:Panel ID="wtsPanelNew" runat="server">
                <div class="control-group">
                    <label class="control-label">Statement</label>
                    <div class="controls">
                        <asp:TextBox ID="wtsStatement" runat="server" TextMode="MultiLine" CssClass="input-xlarge" />
                        <p class="help-block">Give a brief statement about your willingness to serve in this committee.</p>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">Confirm</label>
                    <div class="controls">
                        <label class="checkbox">
                            <asp:CheckBox ID="wtsConfirm" runat="server" ValidationGroup="wts" />
                            I confirm that I am willing to serve as an APSCUF Official.
                        </label>
                        <asp:CustomValidator ID="wtsAcceptValidator" runat="server" 
                            ErrorMessage="Please confirm your willingness to serve by checking the box above." 
                            onservervalidate="wtsAcceptValidator_ServerValidate"
                            CssClass="help-block error"/>
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="wtsSubmit" runat="server" Text="Submit" 
                        onclick="wtsSubmit_Click" CssClass="btn btn-primary" />
                </div>
            </asp:Panel>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="FacultyNomination" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>Primary Election</legend>
            <asp:Panel runat="server" id="TooManyPrimVotes" Visible="false" CssClass="alert alert-warning">
                <strong>Too Many Votes</strong>
                You may vote for up to <asp:Literal runat="server" id="NumVacancies_Prim1" /> nominees(s).
            </asp:Panel>
            <p>Please cast your vote in the primary election for one of the following candidates.</p>
            <div class="control-group">
                <label class="control-label">Candidates</label>
                <div class="controls">
                    <asp:ListView ID="ListViewNom" runat="server">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="nomination_user">
                                <asp:HiddenField id="WTS_ID" Value='<%#Eval("ID") %>' runat="server" />
                                <asp:HiddenField id="WTS_Candidate" Value='<%#Eval("User") %>' runat="server" />
                                <asp:CheckBox id="PrimBallotEntry" runat="server" />
                                <strong><asp:Literal Text='<%#GetName(int.Parse(Eval("User").ToString())) %>' runat="server" /></strong>
                                <p><asp:Literal Text='<%#Eval("Statement") %>' runat="server" /></p>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div class="form-actions">
                <asp:Button ID="FacultyCastNomination" Text="Cast Vote" runat="server"
                    OnClick="FacultyCastNomination_Click" CssClass="btn btn-primary" />
            </div>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="FacultyNominationComplete" runat="server" Visible="false">
        <div class="alert alert-success">
            Thank you for placing your vote in the primary election!
        </div>
    </asp:Panel>

    <asp:Panel ID="FacultyVote" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>General Election</legend>
            <asp:Panel runat="server" id="TooManyGenVotes" Visible="false" CssClass="alert alert-warning">
                <strong>Too Many Votes</strong>
                You may vote for up to <asp:Literal runat="server" id="NumVacancies_Gen1" /> candidates(s).
            </asp:Panel>
            <p>Please cast your vote in the final election for one of the following nominees:</p>
            <div class="control-group">
                <label class="control-label">Candidates</label>
                <div class="controls">
                    <asp:ListView ID="ListViewVote" runat="server">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="nomination_user">
                                <asp:HiddenField id="WTS_ID" Value='<%#Eval("ID") %>' runat="server" />
                                <asp:HiddenField id="WTS_Candidate" Value='<%#Eval("User") %>' runat="server" />
                                <asp:CheckBox id="GenBallotEntry" runat="server" />
                                <strong><asp:Literal Text='<%#GetName(int.Parse(Eval("User").ToString())) %>' runat="server" /></strong>
                                <p><asp:Literal Text='<%#Eval("Statement") %>' runat="server" /></p>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div class="form-actions">
                <asp:Button ID="FacultyCastVote" Text="Cast Vote" runat="server" OnClick="FacultyCastVote_Click"
                    CssClass="btn btn-primary"/>
            </div>
            <script type="text/javascript">
            <!--
            $(":radio").change(function() {
                var t = this;
                var checked = $(":radio:checked");
                checked.each(function(tt) {
                    if(checked[tt] == t)
                        return;
                    checked[tt].checked = false;
                });
            });
            -->
            </script>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="FacultyVoteComplete" runat="server" Visible="false">
        <div class="alert alert-success">
            Thank you for placing your vote in the general election!
        </div>
    </asp:Panel>

</asp:Content>